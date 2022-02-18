using System;
namespace NumRecog
{
    static class LinAlg
    {
        public static double[] vectorMatrixMultiply(double[] vector, double[,] matrix)
        {
            if (vector.Length != matrix.GetLength(1)) throw new FormatException();
            double[] output = new double[matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    output[i] += vector[j] * matrix[i,j];
                }
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
    }
}