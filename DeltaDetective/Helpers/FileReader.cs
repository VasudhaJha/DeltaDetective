using System;
using System.IO;
namespace DeltaDetective.Helpers
{
    /// <summary>
    /// A utility class for reading the content of text files.
    /// </summary>
	public static class FileReader
	{
        /// <summary>
        /// Reads the content of the file and returns it as a string.
        /// </summary>
        /// <returns>The content of the file.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        public static string ReadFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
            using StreamReader reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }
    }
}

