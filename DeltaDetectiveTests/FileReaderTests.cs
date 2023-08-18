using DeltaDetective.Helpers;

namespace DeltaDetectiveTests;

public class FileReaderTests
{
    private string GetTestFilePath(string relativePath)
    {
        // Get the directory where the solution is located
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        // Combine the solution directory with the relative path to the test file
        return Path.Combine(solutionDirectory, "TestFiles", relativePath);
    }

    [Fact]
    public void ReadFileContent_ValidFile_ReturnsContent()
    {
        // Arrange
        string filePath = GetTestFilePath("NonEmptyFile.txt");
        string expectedContent = "This file exists.";

        // Act
        string actualContent = FileReader.ReadFileContent(filePath);

        // Assert
        Assert.Equal(expectedContent, actualContent);
    }

    [Fact]
    public void ReadFileContent_InvalidFile_ThrowsFileNotFoundException()
    {
        // Arrange
        string filePath = GetTestFilePath("NonExistentFile.txt");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => {
            _ = FileReader.ReadFileContent(filePath);
        });
    }
}
