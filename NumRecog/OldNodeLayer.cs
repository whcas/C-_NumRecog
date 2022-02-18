using System;
using System.Collections.Generic;

namespace NumRecog
{
     public class OldNodeLayer
    {
        List<Node> nodes;

        public OldNodeLayer(int numNodes)
        {
            nodes = new List<Node>();
            for (int i = 0; i < numNodes; i++) nodes.Add(new Node());
        }
        public OldNodeLayer(int numNodes, int numNextNodes)
        {
            nodes = new List<Node>();
            for (int i = 0; i < numNodes; i++) nodes.Add(new Node(numNextNodes));
        }

        public List<Node> getNodes()
        {
            return nodes;
        }

        public void setNodeValue(double value, int index)
        {
            nodes[index].setValue(value);
        }

        public double getNodeValue(int nodeIndex)
        {
            return nodes[nodeIndex].getValue();
        }

        public List<double> getNodeValues()
        {
            List<double> values = new List<double>();
            
            foreach (Node n in nodes) values.Add(n.getValue());

            return values;
        }

        public double getWeight(int nodeIndex, int neuronIndex)
        {
            return nodes[nodeIndex].getWeight(neuronIndex);
        }

        public List<double> getWeights()
        {
            List<double> weights = new List<double>();
            for (int i = 0; i < nodes.Count; i++) weights.AddRange(nodes[i].getWeights());
            return weights;
        }

        public List<double> getWeightUpdates()
        {
            List<double> updates = new List<double>();
            foreach (Node node in nodes) updates.AddRange(node.getWeightUpdates());
            return updates;
        }

        public double getBias(int biasIndex)
        {
            return nodes[biasIndex].getBias();
        }

        public List<double> getBiases()
        {
            List<double> biases = new List<double>();
            for (int i = 0; i < nodes.Count; i++) biases.Add(getBias(i));
            return biases;
        }
        
        public List<double> getBiasUpdates()
        {
            List<double> updates = new List<double>();
            foreach (Node node in nodes) updates.Add(node.getBiasUpdate());
            return updates;
        }

        public List<double> getNeurons(int neuronsIndex)
        {
            //untested
            List<double> neuronWeights = new List<double>();
            foreach (Node n in nodes) neuronWeights.Add(n.getWeight(neuronsIndex));
            return neuronWeights;
        }

        public void updateNeurons(List<double> updates)
        {
            int x = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[0].getNuerons().Count; j++, x++)
                {
                    nodes[i].updateNeuron(j, updates[x]);
                }
            }
        }
        public void updateBiases(List<double> updates)
        {
            if (updates == null) throw new ArgumentNullException();
            if (updates.Count != nodes.Count) throw new MLCore.ArgumentWrongSizeException();

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].updateBias(updates[i]);
            }
        }

        public void load(List<int> image)
        {
            if (image == null) throw new ArgumentNullException();
            if (image.Count != nodes.Count) throw new MLCore.ArgumentWrongSizeException();

            List<double> compressedImage = MLCore.compress(image, 256.0);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].setValue(compressedImage[i]);
            }
        }

        public void backpropogate()
        {
            foreach (Node n in nodes)
            {
                
            }
        }

        public void propogateForward(OldNodeLayer nextLayer)
        {
            for (int neuron = 0; neuron < nodes[0].getNuerons().Count; neuron++)
            {
                double total = nextLayer.getNodes()[neuron].getBias();
                for (int node = 0; node < nodes.Count; node++)
                {
                    Node n = nodes[node];
                    total += n.getWeight(neuron) * n.getValue();
                }
                nextLayer.getNodes()[neuron].setValue(MLCore.normalise(total));
            }
        }

        public void report(int ans)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Console.WriteLine("Node " + i + ": " + Math.Round(nodes[i].getValue()*100, 2) + "%");
            }

            Console.WriteLine("Cost = " + cost(ans));
            Console.WriteLine("Best guess = " + guess());
        }

        public double cost(int ans)
        {
            List<double> answers = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                answers.Add(0.0);
            }
            answers[ans] = 1.0;

            return sumSquareDiff(getNodeValues(), answers);
        }

        public static double sumSquareDiff(List<double> a, List<double> b)
        {
            if (a.Count != b.Count) throw new FormatException();

            double diff = 0;
            for (int i = 0; i < a.Count; i++)
            {
                diff += Math.Pow(a[i] - b[i], 2);
            }
            return diff;
        }

        public int guess()
        {
            return highestsIndex(getNodeValues());
        }
        public static int highestsIndex(List<double> guesses)
        {
            List<double> guessesCopy = guesses;
            guessesCopy.Sort();
            double topGuess = guessesCopy[(guessesCopy.Count) - 1];
            return guesses.FindIndex(guesses => topGuess == guesses);
        }
    }
}