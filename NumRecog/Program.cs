using System;
using NumRecog;

namespace MyProgram
{
    class Program
    {
        static string dataBaseFilePath = "C:/Users/Will/Documents/Computer Science Year 1/MyProjects/C# ML/Hand written images/mnist_784.csv";
        static void Main(string[] args)
        {
            ImageSet images = new ImageSet(dataBaseFilePath);

            NeuralNet nn = new NeuralNet(images.getImages(), 60000);

            //Console.WriteLine(nn.testCostRandomImage());

            while (true)
            {
                nn.train(1000, epochLength : 5000, testEveryEpoch : true, epochTestSize : 1000);
            }
        }
    }
}