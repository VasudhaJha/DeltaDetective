using System;
namespace DeltaDetective
{
	public class ProgramFile1
	{
		int a;
		int b;
		public ProgramFile1(int a, int b)
		{
			this.a = a;
			this.b = b;
		}

		private int add()
		{
			return this.a + this.b;
		}
	}
}

