using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumRecog;

namespace Tester
{
    [TestClass]
    public class LinAlgTests
    {
        [TestMethod]
        public void matrixVectorMultiplicationTest()
        {
            double[] vector = {2, 3, 5};
            double[,] matrix = {{4, 2}, {2, 3}, {7, 8}};
            double[] resultingVector = {49, 53};

            double[] experimentalResult = LinAlg.vectorMatrixMultiply(vector, matrix);
            Assert.IsTrue
            (
                LinAlg.vectorsAreEqual(resultingVector, experimentalResult),
                "Result should have been " + resultingVector + " but was " + experimentalResult
            );
        }

        [TestMethod]
        public void vectorAdditionTest()
        {
            double[] v1 = {3, 8, 5};
            double[] v2 = {9, 4, 2};
            double[] result = {
                3 + 9,
                8 + 4,
                5 + 2
            };

            double[] experimentalResult = LinAlg.addVectors(v1, v2);

            Assert.IsTrue(LinAlg.vectorsAreEqual(result, experimentalResult));
        }
    }
}