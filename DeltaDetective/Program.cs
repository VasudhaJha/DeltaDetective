using DeltaDetective.Helpers;
using DeltaDetective.Models;
namespace DeltaDetective;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: DeltaDetective <file1> <file2>");
        }

        TextComparer textComparer = new TextComparer("File1.txt", "File2.txt");
        List<Difference> differences = textComparer.Compare();

        for (int i = differences.Count - 1; i >= 0; i--)
        {
            Difference diff = differences[i];
            Console.WriteLine($"Line Number: {diff.LineNumber}");
            Console.WriteLine($"Difference Type: {diff.Type}");
            Console.WriteLine($"Content in File 1: {diff.ContentInFile1}");
            Console.WriteLine($"Content in File 2: {diff.ContentInFile2}");
        }

        //foreach (Difference diff in differences)
        //{
        //    Console.WriteLine($"Line Number: {diff.LineNumber}");
        //    Console.WriteLine($"Difference Type: {diff.Type}");
        //    Console.WriteLine($"Content in File 1: {diff.ContentInFile1}");
        //    Console.WriteLine($"Content in File 2: {diff.ContentInFile2}");
        //}
    }
}

