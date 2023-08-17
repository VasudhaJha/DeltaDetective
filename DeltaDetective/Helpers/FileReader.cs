using System;
using System.IO;
namespace DeltaDetective.Helpers
{
    /// <summary>
    /// A utility class for reading the content of text files.
    /// </summary>
	public class FileReader
	{
        /// <summary>
        /// Reads the content of the file and returns it as a string.
        /// </summary>
        /// <returns>The content of the file.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        public string ReadFileContent(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found.", filePath);
                }

                using StreamReader reader = new StreamReader(filePath);
                return reader.ReadToEnd();
            }
            catch (FileNotFoundException e)
            {
                // Rethrow the exception to provide more context
                throw new FileNotFoundException($"File not found: {filePath}", e);
            }
        }
    }
}

