using System;
using System.Linq;
using System.Collections.Generic;

namespace NumRecog
{
    public static class MLCore
    {
        public static Random rand = new Random();

        public static List<double> compress(List<int> x, double compressionFactor)
        {
            List<double> compressed = new List<double>();
            foreach (int val in x) compressed.Add( val / compressionFactor );
            return compressed;
        }
        public static double[] compressNew(List<int> x, double compressionFactor)
        {
            double[] compressed = new double[x.Count];
            for (int i = 0; i < x.Count; i++)
            {
                compressed[i] = x[i] / compressionFactor;
            }
            return compressed;
        }

        public static double sumList(List<double> list)
        {
            double sum = 0;
            for (int i = 0; i < list.Count; i++) sum += list[i];
            return sum;
        }

        public static double scalerProduct(List<double> a, List<double> b)
        {
            double product = 0;
            for (int i = 0; i < a.Count; i++) product += (a[i] * b[i]);
            return product;
        }

        public static double normalise(double x)
        {
            return sigmoid(x);
        }

        private static double modifiedSigmoid(double x)
        {
            double y = (sigmoid(x) * 2) - 1;
            return (sigmoid(x) * 2) - 1;
        }

        private static double sigmoid(double x)
        {
            double eToX = Math.Pow(Math.E, x);
            return (eToX) / (eToX + 1);
        }

        internal static double sigmoidDer(double x)
        {
            double sigX = sigmoid(x);
            return (sigX * (1 - sigX));
        }

        internal static double[] sigmoidArray(double[] a)
        {
            double[] output = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                output[i] = sigmoid(a[i]);
            }
            return output;
        }

        internal static double getRandomDouble(int minVal, int maxVal)
        {
            return (rand.NextDouble() * (maxVal - minVal)) + minVal;
        }

        internal static double[] getRandomDoubleArray(int minVal, int maxVal, int len)
        {
            double[] output = new double[len];
            for (int i = 0; i < len; i++)
            {
                output[i] = getRandomDouble(minVal, maxVal);
            }
            return output;
        }
        internal static double[,] getRandomDoubleArray(int minVal, int maxVal, int xLen, int yLen)
        {
            double[,] output = new double[xLen, yLen];
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    output[x, y] = getRandomDouble(minVal, maxVal);
                }
            }
            return output;
        }

        internal static T[] getRandomSubSet<T>(T[] set, int subsetSize)
        {
            List<T> copylist = set.ToList<T>();
            T[] randomSubset = new T[subsetSize];

            for (int i = 0; i < subsetSize; i++)
            {
                int randItemIndex = rand.Next(copylist.Count);
                randomSubset[i] = copylist[randItemIndex];
                copylist.RemoveAt(randItemIndex);
            }

            return randomSubset;
        }

        internal static List<T> shuffle<T>(List<T> list)
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

        public class ArgumentWrongSizeException : Exception {}
    }
}