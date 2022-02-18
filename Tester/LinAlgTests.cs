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
    }
}