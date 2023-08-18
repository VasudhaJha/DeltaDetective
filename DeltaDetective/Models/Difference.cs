using System;
namespace DeltaDetective.Models
{
	public enum DifferenceType
	{
		Added,
		Deleted,
		Replaced
	}
	public class Difference
	{
		public int LineNumber;
		public DifferenceType Type;
		public string ContentInFile1;
		public string ContentInFile2;

		public Difference(int lineNumber, DifferenceType type, string contentInFile1, string contentInFile2)
		{
			LineNumber = lineNumber;
			Type = type;
            ContentInFile1 = contentInFile1;
			ContentInFile2 = contentInFile2;
        }
		
	}
}

