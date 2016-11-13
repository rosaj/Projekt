using System;
namespace Osobni_Troškovnik
{
	public static class StringManipulator
	{
		public static string insertBreaks(string s, int duzina)
		{
			s = s.Trim();
			int range = duzina;
			for (int i = 0; i < s.Length; i++)
			{
				int x = s.IndexOf('\n');
				if (x > 0)
				{
					s = s.Remove(x, 1);
					s = s.Insert(x , " ");
				}
				range += duzina;
			}
			if (s.Length > duzina)
			{
				int i = duzina;
				for (int y = 0; y < s.Length / duzina; y++)
				{
					if (i + duzina < s.Length - 1)
					{
						while (i < s.Length)
						{
							if (s[i] == ' ') break;
							i++;
						}
						if(i<s.Length)	s = s.Insert(i + 1, "\n");
						i += duzina;
					}
				}
			}
			return s;
		}

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
			else return string.Format("{0}",d);
		}

	}
}
