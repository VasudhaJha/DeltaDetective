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
		public int WordNumberInFile1;
        public int WordNumberInFile2;
		public DifferenceType Type;
		public string ContentInFile1;
		public string ContentInFile2;

		public Difference(
			int wordNumberInFile1,
            int wordNumberInFile2,
            DifferenceType type,
			string contentInFile1,
			string contentInFile2)
		{
            WordNumberInFile1 = wordNumberInFile1;
            WordNumberInFile2 = wordNumberInFile2;
            Type = type;
            ContentInFile1 = contentInFile1;
			ContentInFile2 = contentInFile2;
        }
	}
}

