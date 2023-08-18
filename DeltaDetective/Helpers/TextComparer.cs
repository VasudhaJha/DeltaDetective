using System;
using DeltaDetective.Models;
namespace DeltaDetective.Helpers
{
	public class TextComparer
	{
        private string File1;
        private string File2;
		private string _file1Contents;
		private string _file2Contents;

        public TextComparer(string file1Path, string file2Path)
		{
			File1 = file1Path;
			File2 = file2Path;

            try
            {
                _file1Contents = FileReader.ReadFileContent(File1);
                _file2Contents = FileReader.ReadFileContent(File2);
            }
            catch (FileNotFoundException e)
            {
                // Rethrow the exception to provide more context
                throw new FileNotFoundException($"File not found", e);
            }
        }

		public List<Difference> Compare()
		{
            string[] tokens1 = _file1Contents.Split();
            string[] tokens2 = _file2Contents.Split();
            foreach (string token in tokens1)
            {
                Console.WriteLine($"In first file, {token}");
            }

            foreach (string token in tokens2)
            {
                Console.WriteLine($"In second file, {token}");
            }

            Console.WriteLine("-----------------------------");

            int[,] dp = FindLevenshteinDistance(tokens1, tokens2);

			return FindDifferences(tokens1, tokens2, dp);
        }

		private int[,] FindLevenshteinDistance(string[] tokens1, string[] tokens2)
		{
            int[,] dp = new int[tokens1.Length + 1, tokens2.Length + 1];
            dp[0, 0] = 0;

            for (int i = 1; i <= tokens1.Length; i++)
            {
                dp[i, 0] = tokens1[i - 1].Length;
            }

            for (int j = 1; j <= tokens2.Length; j++)
            {
                dp[0, j] = tokens2[j - 1].Length;
            }

            for (int i = 1; i <= tokens1.Length; i++)
            {
                for (int j = 1; j <= tokens2.Length; j++)
                {
                    if (tokens1[i - 1] == tokens2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {
                        dp[i, j] = Math.Min(Math.Min(dp[i, j - 1] + 1, dp[i - 1, j] + 1), dp[i - 1, j - 1] + 1);
                    }
                }
            }

            return dp;
        }

        private List<Difference> FindDifferences(string[] tokens1, string[] tokens2, int[,] dp)
		{
			int i = tokens1.Length;
			int j = tokens2.Length;
            List<Difference> differences = new List<Difference>();

			while (i > 0 && j > 0)
			{
				if (tokens1[i - 1] == tokens2[j - 1]){
					// these words are identical, so check the previous two words.
					i--;
					j--;
				}
				else if (dp[i, j] == dp[i - 1, j - 1] + 1)
				{
					// Replacement case
                    differences.Add(new Difference(
                        wordNumber: i,
                        type: DifferenceType.Replaced,
                        contentInFile1: tokens1[i - 1],
                        contentInFile2: tokens2[j - 1]
                        ));
					i--;
					j--;
				}
                else if (dp[i, j] == dp[i - 1, j] + 1)
                {
					// When the word in file1 was deleted
                    differences.Add(new Difference(
                        wordNumber: i,
                        type: DifferenceType.Deleted,
                        contentInFile1: tokens1[i - 1],
                        contentInFile2: ""
                        ));
                    i--;
                }
                else if (dp[i, j] == dp[i, j - 1] + 1)
                {
					// When a word was inserted in file2
                    differences.Add(new Difference(
                        wordNumber: i,
                        type: DifferenceType.Added,
                        contentInFile1: "",
                        contentInFile2: tokens2[j - 1]
                        ));
                    j--;
                }
            }

            while (i > 0)
            {
				// words deleted from file 1
                differences.Add(new Difference(
                        wordNumber: i,
                        type: DifferenceType.Deleted,
                        contentInFile1: tokens1[i - 1],
                        contentInFile2: ""
                        ));
                i--;
            }

            while (j > 0)
            {
				// words added in file2
                differences.Add(new Difference(
                        wordNumber: i,
                        type: DifferenceType.Added,
                        contentInFile1: "",
                        contentInFile2: tokens2[j - 1]
                        ));
                j--;
            }

            return differences;
        }
	}
}

