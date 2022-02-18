using System.Collections.Generic;

namespace NumRecog
{
    public class Node
    {
        List<Neuron> neurons;
        double value;
        double bias;
        double biasUpdate;

        public Node()
        {
            setRandomBias();
        }
        public Node(int numNeurons)
        {
            randomiseNeurons(numNeurons);
            setRandomBias();
        }

        protected void randomiseNeurons(int numNeurons)
        {
            neurons = new List<Neuron>();

            for (int i = 0; i < numNeurons; i++)
            {
                double randomWeight = (MLCore.rand.NextDouble() - 0.5) * 20;
                neurons.Add(new Neuron(randomWeight));
            }
        }

        protected void setRandomBias()
        {
            double randomBias = ((MLCore.rand.NextDouble() - 0.5) * 20);
            bias = randomBias;
        }

        public void setValue(double value)
        {
            this.value = value;
        }

        public void updateNeuron(int index, double update)
        {
            neurons[index].update(update);
        }
        public void updateBias(double update)
        {
            bias += update;
        }

        public List<Neuron> getNuerons()
        {
            return neurons;
        }
        public List<double> getWeightUpdates()
        {
            List<double> updates = new List<double>();
            foreach (Neuron n in neurons)
            {
                updates.Add(n.getUpdate());
            }
            return updates;
        }
        public double getValue()
        {
            return value;
        }

        public double getWeight(int neuronIndex)
        {
            return neurons[neuronIndex].getWeight();
        }

        public List<double> getWeights()
        {
            List<double> weights = new List<double>();
            for (int i = 0; i < neurons.Count; i++) weights.Add(getWeight(i));
            return weights;
        }

        public double getBias()
        {
            return bias;
        }
        public double getBiasUpdate()
        {
            return biasUpdate;
        }
    }
}