using System.Numerics;
using System;
namespace NumRecog
{
    static public class LinAlg
    {
        public static double[] vectorMatrixMultiply(double[] vector, double[,] matrix)
        {
            if (vector.Length != matrix.GetLength(0)) throw new FormatException();
            double[] output = new double[matrix.GetLength(1)];
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                output = addVectors(output, vectorScaling(getColumn(matrix, i), vector[i]));
            }
            return output;
        }

        private static double[] addVectors(double[] v1, double[] v2)
        {
            double[] output = new double[v1.Length];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = v1[i] + v2[i];
            }
            return output;
        }

        private static double[] getColumn(double[,] matrix, int index)
        {
            if (matrix.GetLength(0) < index) throw new FormatException();
            
            double[] output = new double[matrix.GetLength(1)];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = matrix[index, i];
            }
            return output;
        }

        public static double[] vectorScaling(double[] vector, double scaler)
        {
            double[] output = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                output[i] = vector[i] * scaler;
            }
            return output;
        }
        public static double sumSquareDiff(double[] a, double[] b)
        {
            if (a.Length != b.Length) throw new FormatException();

            double diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff += Math.Pow(a[i] - b[i], 2);
            }
            return diff;
        }

        public static bool vectorsAreEqual(double[] v1, double[] v2)
        {
            if (v1.Length != v2.Length) return false;

            bool output = true;
            for (int i = 0; i < v1.Length; i++)
            {
                if (v1[i] != v2[i]) output = false;
            }
            return output;
        }
    }
}