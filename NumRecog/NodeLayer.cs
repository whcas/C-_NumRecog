using System.ComponentModel.DataAnnotations;
namespace NumRecog
{
    public class NodeLayer
    {
        private NodeLayer previous = null;

        private double[] activations;
        private bool activationsCalculated = false;
        private double[,] weights;
        private double[,] weightCostDers = null;
        private double[] biases;
        private double[] biasCostDers = null;

        private double[] z;

        private double[] actCostDers = null;
        private double[] zActDers = null;


        public NodeLayer(int numMoreLayers, int inputLayerSize, int middleLayerSize, int outputLayerSize)
        {
            if (numMoreLayers < 1) throw new ValidationException("Neural Network needs at least 2 layers");

            activations = new double[outputLayerSize];

            previous = new NodeLayer(numMoreLayers - 1, inputLayerSize, middleLayerSize, numMoreLayers == 1);
            generateWeights(outputLayerSize);
            generateBiases(outputLayerSize);

        }

        internal NodeLayer(int numMoreLayers, int inputLayerSize, int middleLayerSize, bool inputLayer)
        {
            int numNodes = middleLayerSize;
            if (inputLayer) numNodes = inputLayerSize;

            activations = new double[numNodes];

            if (!inputLayer)
            {
                previous = new NodeLayer(numMoreLayers - 1, inputLayerSize, middleLayerSize, numMoreLayers == 1);
                generateWeights(numNodes);
                generateBiases(numNodes);
            }
        }

        internal double[] getAnswer(ImageBits image)
        {
            backpropogate(MLCore.compressNew(image.getPixels(), 256.0), image.getLabel());

            return getActivations();
        }

        internal bool isInputLayer() { return (previous == null); }

        protected int getNumActivations()
        {
            return activations.Length;
        }

        private void generateWeights(int numNodes)
        {
            weights = MLCore.getRandomDoubleArray(0, 1, numNodes, previous.getNumActivations());
        }
        private void generateBiases(int numNodes)
        {
            biases = MLCore.getRandomDoubleArray(-10, 10, numNodes);
        }

        public double cost(int ans)
        {
            if (activationsCalculated == false) calcActivations();

            double[] answers = new double[weights.GetLength(0)];
            answers[ans] = 1.0;

            return LinAlg.sumSquareDiff(activations, answers);
        }

        public double testCostAverage(ImageBits[] testImages)
        {
            double costTotal = 0;
            for (int i = 0; i < testImages.Length; i++)
            {
                setInputLayer(MLCore.compressNew(testImages[i].getPixels(), 256.0));
                costTotal += cost(testImages[i].getLabel());
            }
            return costTotal / testImages.Length;
        }

        private void calcActivations()
        {
            double[] previousActivations = previous.getActivations();

            z = LinAlg.vectorMatrixMultiply(previousActivations, weights);

            activations = MLCore.sigmoidArray(z);

            activationsCalculated = true;
        }

        private double[] getActivations()
        {
            if (activationsCalculated == false) calcActivations();
            return activations;
        }

        public void setInputLayer(double[] inputs)
        {
            if (previous == null)
            {
                activations = inputs;
                activationsCalculated = true;
            }
            else
            {
                activationsCalculated = false;
                previous.setInputLayer(inputs);
            }
        }

        public void backpropogate(double[] input, int ans)
        {
            setInputLayer(input);
            if (activationsCalculated == false) calcActivations();

            double[] answers = new double[activations.GetLength(0)];
            answers[ans] = 1.0;

            if (actCostDers == null) calcActCostDers(answers);

            if (zActDers == null) calcZActDers();

            if (weightCostDers == null) calcWeightCostDers();

            if (biasCostDers == null) calcBiasCostDers();

            if (!previous.isInputLayer()) previous.backpropogate(zActDers, actCostDers, weights);

            actCostDers = null;
            zActDers = null;
        }

        private void backpropogate(double[] nextZActDers, double[] nextActCostDers, double[,] nextWeights)
        {
            if (previous != null)
            {
                calcActCostDers(nextZActDers, nextActCostDers, nextWeights);
                calcZActDers();
                calcWeightCostDers();
                calcBiasCostDers();
                if (!previous.isInputLayer()) previous.backpropogate(zActDers, actCostDers, weights);
            }
        }

        private void calcActCostDers(double[] nextZActDers, double[] nextActCostDers, double[,] nextWeights)
        {
            actCostDers = activations;
            for (int k = 0; k < activations.Length; k++)
            {
                for (int j = 0; j < nextWeights.GetLength(0); j++)
                {
                    actCostDers[k] += (nextWeights[j, k] * nextZActDers[j] * nextActCostDers[j]);
                }
            }
        }

        private void calcWeightCostDers()
        {
            double[] previousActivations = previous.getActivations();

            weightCostDers = weights;
            for (int j = 0; j < weights.GetLength(0); j++)
            {
                for (int k = 0; k < weights.GetLength(1); k++)
                {
                    weightCostDers[j, k] =
                    (
                        previousActivations[k] *
                        zActDers[j] *
                        actCostDers[j]
                    );
                }
            }
        }

        private void calcBiasCostDers()
        {
            biasCostDers = biases;
            for (int j = 0; j < biases.Length; j++)
            {
                biasCostDers[j] = (zActDers[j] * actCostDers[j]);
            }
        }

        private void calcZActDers()
        {
            zActDers = new double[z.Length];
            for (int i = 0; i < z.Length; i++)
            {
                zActDers[i] = MLCore.sigmoidDer(z[i]);
            }

        }

        private void calcActCostDers(double[] answers)
        {
            actCostDers = new double[answers.Length];
            for (int i = 0; i < activations.Length; i++)
            {
                actCostDers[i] = 2 * (activations[i] - answers[i]);
            }
        }



        public WeightAndBiasChanges[] getAllWeightAndBiasChanges()
        {
            WeightAndBiasChanges[] output = new WeightAndBiasChanges[getDepth()];
            output[0] = getMyWeightAndBiasChanges();
            if (!previous.isInputLayer())
            {
                WeightAndBiasChanges[] previousOutput = previous.getAllWeightAndBiasChanges();

                for (int i = 1; i < output.Length; i++)
                {
                    output[i] = previousOutput[i - 1];
                }
            }
            return output;
        }

        private WeightAndBiasChanges getMyWeightAndBiasChanges()
        {
            WeightAndBiasChanges mine = new WeightAndBiasChanges();
            mine.weightChanges = weightCostDers;
            weightCostDers = null;
            mine.biasChanges = biasCostDers;
            biasCostDers = null;
            return mine;
        }

        public void updateWeightsAndBiases(WeightAndBiasChanges[] updates)
        {
            updateMyWeightsAndBiases(updates[0]);

            if (!previous.isInputLayer())
            {
                WeightAndBiasChanges[] previousUpdates = new WeightAndBiasChanges[updates.Length - 1];
                for (int i = 1; i < updates.Length; i++)
                {
                    previousUpdates[i - 1] = updates[i];
                }
                previous.updateWeightsAndBiases(previousUpdates);
            }
        }

        private void updateMyWeightsAndBiases(WeightAndBiasChanges updates)
        {
            updateWeights(updates.weightChanges);
            updateBiases(updates.biasChanges);
        }

        private void updateWeights(double[,] weightChanges)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] -= weightChanges[i, j];
                }
            }
        }

        private void updateBiases(double[] biasChanges)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] -= biasChanges[i];
            }
        }

        private int getDepth()
        {
            if (previous == null) return 0;

            return previous.getDepth() + 1;
        }
    }
}