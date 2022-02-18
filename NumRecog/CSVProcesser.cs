using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NumRecog
{
    public static class CSVProcess
    {
        public static List<List<string>> toArray(string fileName)
        {
            string wholeFile = File.ReadAllText(fileName);

            wholeFile = wholeFile.Replace('\n', '\r');
            List<string> lines = wholeFile.Split(new char[] { '\r' }).ToList();

            List<List<string>> output = new List<List<string>>();
            for (int row = 0; row < lines.Count; row++)
            {
                output.Add(lines[row].Split(',').ToList());
            }

            return output;
        }

        public static List<List<int>> toIntArray(string fileName)
        {
            List<List<string>> stringArray = toArray(fileName);

            List<List<int>> intArray = new List<List<int>>();
            foreach (List<string> entry in stringArray)
            {
                intArray.Add(parseList(entry));
            }

            return intArray;
        }

        private static List<int> parseList(List<string> stringList)
        {
            List<int> intList = new List<int>();
            foreach (string value in stringList)
            {
                intList.Add(int.Parse(value));
            }
            return intList;
        }
    }
}