using System;
namespace Osobni_Troškovnik
{
	public static class StringManipulator
	{
		

		public static string formatter(double d)
		{
			if (d < 1E3)
			{
				return string.Format("{0}", d);
			}
			else if (d < 1E6)
			{
				return string.Format("{0}K", d / 1E3);
			}
			else if (d < 1E9)
			{
				return string.Format("{0}M", d / 1E6);
			}
			else if (d > 1E9)
			{
				return string.Format("{0}B", d / 1E9);
			}
			else return string.Format("{0}", d);
		}

		public static string convertToMjesec(double mjesec)
		{
			string mjesecString = "NA";
			switch ((int)mjesec)
			{
				case 1:
					mjesecString = "Siječanj";
					break;
				case 2:
					mjesecString = "Veljača";
					break;
				case 3:
					mjesecString = "Ožujak";
					break;
				case 4:
					mjesecString = "Travanj";
					break;
				case 5:
					mjesecString = "Svibanj";
					break;
				case 6:
					mjesecString = "Lipanj";
					break;
				case 7:
					mjesecString = "Srpanj";
					break;
				case 8:
					mjesecString = "Kolovoz";
					break;
				case 9:
					mjesecString = "Rujan";
					break;
				case 10:
					mjesecString = "Listopad";
					break;
				case 11:
					mjesecString = "Studeni";
					break;
				case 12:
					mjesecString = "Prosinac";
					break;
			}
			return mjesecString;
		}
	}
}
