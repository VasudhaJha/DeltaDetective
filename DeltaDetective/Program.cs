using System;
using DeltaDetective.Helpers;
namespace DeltaDetective;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: DeltaDetective <Original File> <Changed File>");
            return;
        }
        string file1Path = args[0];
        string file2Path = args[1];
        try
        {
            TextComparer textComparer = new TextComparer(file1Path, file2Path);
            textComparer.Compare();
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
        
    }
}

