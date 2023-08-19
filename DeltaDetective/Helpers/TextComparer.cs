using System;
using DeltaDetective.Models;
namespace DeltaDetective.Helpers
{
    /// <summary>
    /// Compares two text files and identifies differences between their content.
    /// Differences include added, removed, and replaced words between the files.
    /// The class provides methods to perform comparison and retrieve differences.
    /// </summary>
    public class TextComparer
	{
        private string _file1;
        private string _file2;
		private string _file1Contents;
		private string _file2Contents;
        private int _additions;
        private int _removals;

        public TextComparer(string file1Path, string file2Path)
		{
            _file1 = file1Path;
            _file2 = file2Path;
            _file1Contents = FileReader.ReadFileContent(file1Path);
            _file2Contents = FileReader.ReadFileContent(file2Path);   
        }

        /// <summary>
        /// Compares the content of two text files and identifies differences between them.
        /// The method uses the Levenshtein distance algorithm to find differences at the word level.
        /// Differences include added, removed, and replaced words between the files.
        /// The differences are displayed to the console with appropriate formatting.
        /// </summary>
        public void Compare()
		{
            string[] tokens1 = _file1Contents.Split();
            string[] tokens2 = _file2Contents.Split();

            int[,] dp = FindLevenshteinDistance(tokens1, tokens2);

			List<Difference> differences = FindDifferences(tokens1, tokens2, dp);

            DisplayDifferences(tokens1, tokens2, differences);
        }

        /// <summary>
        /// Computes the Levenshtein distance between two arrays of tokens using dynamic programming.
        /// The Levenshtein distance represents the minimum number of edits (insertions, deletions,
        /// and substitutions) required to transform one array of tokens into the other.
        /// Returns a 2D matrix containing the Levenshtein distances between all pairs of tokens.
        /// </summary>
        /// <param name="tokens1">The array of tokens from the first file.</param>
        /// <param name="tokens2">The array of tokens from the second file.</param>
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

        /// <summary>
        /// Finds and returns a list of differences between two arrays of tokens based on their Levenshtein distances.
        /// Compares the two arrays of tokens using the Levenshtein distances calculated earlier and identifies
        /// words that were added, removed, or replaced between the two arrays.
        /// </summary>
        /// <param name="tokens1">The array of tokens from the first file.</param>
        /// <param name="tokens2">The array of tokens from the second file.</param>
        /// <param name="dp">A 2D matrix of Levenshtein distances between tokens.</param>
        /// <returns>A list of differences between the two arrays of tokens.</returns>
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
                    //Console.WriteLine($"Replaced '{tokens1[i - 1]}' with '{tokens2[j - 1]}'");
                    differences.Add(new Difference(
                        wordNumberInFile1: i - 1,
                        wordNumberInFile2: j - 1,
                        type: DifferenceType.Replaced,
                        contentInFile1: tokens1[i - 1],
                        contentInFile2: tokens2[j - 1]
                        ));
                    _additions++;
                    _removals++;
                    i--;
                    j--;
                }
                else if (dp[i, j] == dp[i - 1, j] + 1)
                {
                    // When the word in file1 was deleted
                    //Console.WriteLine($"Deleted '{tokens1[i - 1]}' from file 1");
                    differences.Add(new Difference(
                        wordNumberInFile1: i - 1,
                        wordNumberInFile2: j,
                        type: DifferenceType.Deleted,
                        contentInFile1: tokens1[i - 1],
                        contentInFile2: ""
                        ));
                    _removals++;
                    i--;
                }
                else if (dp[i, j] == dp[i, j - 1] + 1)
                {
                    // When a word was inserted in file2
                    //Console.WriteLine($"Added '{tokens2[j - 1]}' in file 2");
                    differences.Add(new Difference(
                        wordNumberInFile1: i,
                        wordNumberInFile2: j - 1,
                        type: DifferenceType.Added,
                        contentInFile1: "",
                        contentInFile2: tokens2[j - 1]
                        ));
                    _additions++;
                    j--;
                }
            }

            while (i > 0)
            {
                // words deleted from file 1
                //Console.WriteLine($"Deleted '{tokens1[i - 1]}' from file 1");
                differences.Add(new Difference(
                        wordNumberInFile1: i - 1,
                        wordNumberInFile2: j - 1,
                        type: DifferenceType.Deleted,
                        contentInFile1: tokens1[i - 1],
                        contentInFile2: ""
                        ));
                _removals++;
                i--;
            }

            while (j > 0)
            {
                // words added in file2
                //Console.WriteLine($"Added '{tokens2[j - 1]}' in file 2");
                differences.Add(new Difference(
                        wordNumberInFile1: i - 1,
                        wordNumberInFile2: j - 1,
                        type: DifferenceType.Added,
                        contentInFile1: "",
                        contentInFile2: tokens2[j - 1]
                        ));
                _additions++;
                j--;
            }

            return differences;
        }

        /// <summary>
        /// Displays the differences between two arrays of tokens along with their highlighted content in the console.
        /// Compares the two arrays of tokens and the list of differences to present the modified, added, and removed
        /// words in a visually informative way. Uses colored text and symbols to highlight the changes.
        /// </summary>
        /// <param name="tokens1">The array of tokens from the first file.</param>
        /// <param name="tokens2">The array of tokens from the second file.</param>
        /// <param name="differences">A list of differences between the two arrays of tokens.</param>
        private void DisplayDifferences(string[] tokens1, string[] tokens2, List<Difference> differences)
        {
            string absoluteFilePath1 = Path.GetFullPath(_file1);
            string absoluteFilePath2 = Path.GetFullPath(_file2);

            Console.Write($"--- a{absoluteFilePath1} ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{_removals} removals");
            Console.ResetColor();

            Console.Write($"+++ b{absoluteFilePath2} ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_additions} additions");
            Console.ResetColor();

            Console.WriteLine("\n");

            for (int i = 0; i < tokens1.Length; i++)
            {
                Difference diff = differences.FirstOrDefault(d => d.WordNumberInFile1 == i);
                
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
                Difference diff = differences.FirstOrDefault(d => d.WordNumberInFile2 == j);

                if (diff != null)
                {
                    //Console.WriteLine("\n" + "DIFF WORD IS: " + diff.ContentInFile2 + " WITH TYPE " + diff.Type.ToString());
                    switch (diff.Type)
                    {
                        case DifferenceType.Added:
                            Console.BackgroundColor = Constants.AddedBackgroundColor;
                            break;
                        case DifferenceType.Replaced:
                            //Console.WriteLine("HERE!");
                            //Console.WriteLine(tokens2[j]);
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

