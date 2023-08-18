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

		public void Compare()
		{
            string[] tokens1 = _file1Contents.Split();
            string[] tokens2 = _file2Contents.Split();

            int[,] dp = FindLevenshteinDistance(tokens1, tokens2);

			List<Difference> differences = FindDifferences(tokens1, tokens2, dp);

            DisplayDifferences(tokens1, tokens2, differences);
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
                    // When the word in file1 was replaced with a different word in file2
                    differences.Add(new Difference(
                        wordNumber: i - 1,
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
                        wordNumber: i - 1,
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
                        wordNumber: j - 1,
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
                        wordNumber: i - 1,
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
                        wordNumber: j - 1,
                        type: DifferenceType.Added,
                        contentInFile1: "",
                        contentInFile2: tokens2[j - 1]
                        ));
                j--;
            }

            return differences;
        }

        private void DisplayDifferences(string[] tokens1, string[] tokens2, List<Difference> differences)
        {
            string absoluteFilePath1 = Path.GetFullPath(File1);
            string absoluteFilePath2 = Path.GetFullPath(File2);
            Console.WriteLine($"--- a{absoluteFilePath1}");
            Console.WriteLine($"+++ b{absoluteFilePath1}");
            Console.WriteLine("\n");
            for (int i = 0; i < tokens1.Length; i++)
            {
                Difference diff = differences.FirstOrDefault(d => d.WordNumber == i);
                
                if (diff != null)
                {
                    switch (diff.Type)
                    {
                        case DifferenceType.Deleted:
                            Console.BackgroundColor = Constants.DeletedBackgroundColor;
                            break;
                        case DifferenceType.Replaced:
                            Console.BackgroundColor = Constants.DeletedBackgroundColor;
                            break;
                        default:
                            break;
                    }
                }
                Console.Write(tokens1[i] + " ");
                Console.ResetColor();
            }
            Console.WriteLine("\n");
            for (int j = 0; j < tokens2.Length; j++)
            {
                Difference diff = differences.FirstOrDefault(d => d.WordNumber == j);

                if (diff != null)
                {
                    switch (diff.Type)
                    {
                        case DifferenceType.Added:
                            Console.BackgroundColor = Constants.AddedBackgroundColor;
                            break;
                        case DifferenceType.Replaced:
                            Console.BackgroundColor = Constants.AddedBackgroundColor;
                            break;
                        default:
                            break;
                    }
                }
                Console.Write(tokens2[j] + " ");
                Console.ResetColor();
            }
            Console.WriteLine("\n");
        }
	}
}

