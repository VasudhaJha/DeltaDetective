using DeltaDetective.Helpers;
namespace DeltaDetectiveTests
{
	public class TextComparerTests
	{
        private string GetTestFilePath(string relativePath)
        {
            // Get the directory where the solution is located
            string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            // Combine the solution directory with the relative path to the test file
            return Path.Combine(solutionDirectory, "TestFiles", relativePath);
        }

        [Fact]
        public void TextComparer_InitializationWithInvalidFile_ThrowsFileNotFoundException()
        {
            // Arrange
            string file1Path = GetTestFilePath("NonEmptyFile.txt");
            string file2Path = GetTestFilePath("NonExistantFile.txt");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                TextComparer textComparer = new TextComparer(file1Path, file2Path);
            });
        }

        // TODO: Add more tests for comparison
    }
}

