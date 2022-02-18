using System;
using System.Linq;
using System.Diagnostics;

namespace NumRecog
{
    class NeuralNet
    {
        internal NodeLayer layers;

        internal ImageBits[] trainingImages;
        internal ImageBits[] testingImages;


        public NeuralNet(ImageBits[] imageSet, int trainingNum)
        {
            layers = new NodeLayer(4, 784, 16, 10);

            trainingImages = imageSet[0..trainingNum];

            testingImages = imageSet[trainingNum..imageSet.Length];
        }

        public double testCostRandomImage()
        {
            ImageBits testImage = testingImages[MLCore.rand.Next(0, testingImages.Length - 1)];
            double[] testImageData = MLCore.compressNew(testImage.getPixels(), 256.0);

            layers.setInputLayer(testImageData);

            return layers.cost(testImage.getLabel());
        }

        public double testCostAverage()
        {
            double totalCost = 0;
            for (int i = 0; i < testingImages.Length; i++)
            {
                layers.setInputLayer(MLCore.compressNew(testingImages[i].getPixels(), 256.0));
                totalCost += layers.cost(testingImages[i].getLabel());
            }
            return totalCost / testingImages.Length;
        }

        public void train(int stochasticInterval, int epochLength = -1, bool testEveryEpoch = false, int epochTestSize = 0)
        {
            WeightAndBiasChanges[][] updates = new WeightAndBiasChanges[stochasticInterval][];
            
            Console.WriteLine("At start of training network cost average = " + layers.testCostAverage(testingImages));

            Stopwatch timer = Stopwatch.StartNew();
            long totalTime = 0;

            ImageBits[] shuffledTrainingImages = MLCore.shuffle(trainingImages.ToList()).ToArray();
            for (int i = 0; i < trainingImages.Length; i++)
            {
                double[] imageAsInputs = MLCore.compressNew(trainingImages[i].getPixels(), 256.0);
                layers.backpropogate(imageAsInputs, trainingImages[i].getLabel());

                updates[i % stochasticInterval] = layers.getAllWeightAndBiasChanges();

                if (i % stochasticInterval == stochasticInterval - 1)
                {
                    WeightAndBiasChanges[] averageUpdates = WeightAndBiasChanges.getAverage(updates);
                    layers.updateWeightsAndBiases(averageUpdates);
                    
                    Console.WriteLine("Time elapsed between stochastic intervals = " + timer.ElapsedMilliseconds + "ms");
                    totalTime += timer.ElapsedMilliseconds;
                    timer.Restart();
                }

                if (testEveryEpoch && (epochLength != -1) && (i % epochLength == 0))
                {
                    ImageBits[] randomTestImages = MLCore.getRandomSubSet(testingImages, epochTestSize);
                    Console.WriteLine("Average Cost at Epoch: " + (i/epochLength) + " = " + layers.testCostAverage(randomTestImages));
                }
            }

            Console.WriteLine("At end of training network cost average = " + layers.testCostAverage(testingImages));
            
            answerRandomImage();
        }

        private void answerRandomImage()
        {
            ImageBits testImage = testingImages[MLCore.rand.Next(testingImages.Length)];
            double[] answers = layers.getAnswer(testImage);
            for (int i = 0; i < answers.Length; i++)
            {
                Console.WriteLine("confidence image is a " + i + ": " + answers[i] * 100 + "%");
            }
            Console.WriteLine("\nImage was a " + testImage.getLabel());
        }
    }
}