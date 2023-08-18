using System;
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
        textComparer.Compare();
    }
}

