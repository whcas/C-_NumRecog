using System.Linq;
using System;
using System.Collections.Generic;

namespace NumRecog
{
    public class OldNeuralNet
    {
        OldNodeLayer inputLayer;
        List<OldNodeLayer> middleLayers;
        OldNodeLayer outputLayer;

        public OldNeuralNet(int numMiddleLayers, int middleLayerSize, int inputLayerSize = 784, int outputLayerSize = 10)
        {
            inputLayer = new OldNodeLayer(inputLayerSize, middleLayerSize);

            middleLayers = new List<OldNodeLayer>();
            for (int i = 0; i < numMiddleLayers - 1; i++)
            {
                OldNodeLayer newLayer = new OldNodeLayer(middleLayerSize, middleLayerSize);
                middleLayers.Add(newLayer);
            }
            OldNodeLayer lastMiddleLayer = new OldNodeLayer(middleLayerSize, outputLayerSize);
            middleLayers.Add(lastMiddleLayer);

            outputLayer = new OldNodeLayer(outputLayerSize);
        }

        public void run(ImageBits image)
        {
            inputLayer.load(image.getPixels());
            inputLayer.propogateForward(middleLayers[0]);

            for (int i = 0; i < middleLayers.Count - 1; i++)
            {
                middleLayers[i].propogateForward(middleLayers[i+1]);
            }
            middleLayers[middleLayers.Count-1].propogateForward(outputLayer);
        }
        public bool test(ImageBits image, bool report = false)
        {
            run(image);

            if (report) outputLayer.report(image.getLabel());

            return outputLayer.guess() == image.getLabel();
        }
        public double test(List<ImageBits> testData)
        {
            int numCorrect = 0;
            foreach (ImageBits image in testData)
            {
                if (test(image)) { numCorrect ++; }
            }

            return numCorrect / testData.Count;
        }

        public void train(List<ImageBits> trainingData, int batchSize, int epoch = 0, bool testing = false, List<ImageBits> testData = null)
        {
            trainingData = shuffle<ImageBits>(trainingData);

            for (int i = 0; i < trainingData.Count / batchSize; i++)
            {
                List<ImageBits> batchedTrainingData = trainingData.GetRange(i * batchSize, batchSize);
                step(batchedTrainingData);

                if (testing && testData != null)
                {
                    Console.WriteLine("Current score: " + test(testData) * 100 + "%");
                }
            }
            Console.WriteLine("Current score: " + test(testData) * 100 + "%");
        }
        private void step(List<ImageBits> stepTrainingData)
        {
            List<List<double>> updates = computeUpdate(stepTrainingData);

            update(updates);
        }
        private List<List<double>> computeUpdate(List<ImageBits> trainingData)
        {
            List<List<double>> weightUpdatesTotal = new List<List<double>>();
            List<List<double>> biasUpdatesTotal = new List<List<double>>();

            foreach (ImageBits image in trainingData)
            {
                List<List<double>> imageUpdates = computeImage(image);

                weightUpdatesTotal.Add(imageUpdates[0]);
                biasUpdatesTotal.Add(imageUpdates[1]);
            }

            List<double> weightUpdatesAverage = averageList(weightUpdatesTotal);
            List<double> biasUpdatesAverage = averageList(biasUpdatesTotal);
            return new List<List<double>>() {weightUpdatesAverage, biasUpdatesAverage};
        }
        private List<List<double>> computeImage(ImageBits image)
        {
            run(image);
            outputLayer.backpropogate();
            return scrape();
        }
        private List<List<double>> scrape()
        {
            List<double> weightUpdates = new List<double>();
            List<double> biasUpdates = new List<double>();

            List<OldNodeLayer> layers = new List<OldNodeLayer>();
            layers.Add(inputLayer);
            layers.AddRange(middleLayers);
            layers.Add(outputLayer);

            for (int i = 0; i < layers.Count; i++)
            {
                weightUpdates.AddRange(layers[i].getWeightUpdates());
                biasUpdates.AddRange(layers[i].getBiasUpdates());
            }

            return new List<List<double>>() {weightUpdates, biasUpdates};
        }
        protected static List<double> averageList(List<List<double>> list)
        {
            List<double> output = new List<double>();
            for (int i = 0; i < list[0].Count; i++)
            {
                double total = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    total += list[j][i];
                }
                output.Add(total / list.Count);
            }
            return output;
        }
        protected static List<T> shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = MLCore.rand.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
            return list;
        }

        public void updateWeights(List<double> weightUpdates)
        {
            int weightRangeStart = 0;
            int inputLayerSize = inputLayer.getNodes().Count;
            int middleLayerSize = middleLayers[0].getNodes().Count;
            int outputLayerSize = outputLayer.getNodes().Count;
            List<double> inputWeightUpdates = weightUpdates.GetRange(weightRangeStart, inputLayerSize * middleLayerSize);
            inputLayer.updateNeurons(inputWeightUpdates);

            for (int i = 0; i < middleLayers.Count - 1; i++)
            {
                weightRangeStart = (i * (int)Math.Pow(middleLayerSize, 2)) + (inputLayerSize * middleLayerSize);
                List<double> middleWeightUpdates = weightUpdates.GetRange(weightRangeStart, (int)Math.Pow(middleLayerSize, 2));
                middleLayers[i].updateNeurons(middleWeightUpdates);
            }
            weightRangeStart = ((middleLayers.Count - 1) * (int)Math.Pow(middleLayerSize, 2)) + (inputLayerSize * middleLayerSize);
            List<double> finalWeightUpdates = weightUpdates.GetRange(weightRangeStart, middleLayerSize * outputLayerSize);
            middleLayers[middleLayers.Count - 1].updateNeurons(finalWeightUpdates);
        }
        public void updateBiases(List<double> biasUpdates)
        {
            int biasRangeStart = 0;
            int inputLayerSize = inputLayer.getNodes().Count;
            int middleLayerSize = middleLayers[0].getNodes().Count;
            int outputLayerSize = outputLayer.getNodes().Count;
            List<double> inputBiasUpdates = biasUpdates.GetRange(biasRangeStart, inputLayerSize);
            inputLayer.updateBiases(inputBiasUpdates);

            for (int i = 0; i < middleLayers.Count; i++)
            {
                biasRangeStart = (i * middleLayerSize) + inputLayerSize;
                List<double> middleBiasUpdates = biasUpdates.GetRange(biasRangeStart, middleLayerSize);
                middleLayers[i].updateBiases(middleBiasUpdates);
            }

            biasRangeStart = (middleLayers.Count * middleLayerSize) + inputLayerSize;
            List<double> outputBiasUpdates = biasUpdates.GetRange(biasRangeStart, outputLayerSize);
            outputLayer.updateBiases(outputBiasUpdates);
        }

        public void update(List<List<double>> updates)
        {
            updateWeights(updates[0]);
            updateBiases(updates[1]);
        }
    }
}