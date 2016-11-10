using System;
namespace Osobni_Troškovnik
{
	public static class StringManipulator
	{
		public static string insertBreaks(string s, int duzina)
		{
			s = s.Trim();
			int range = duzina;
			for (int i = 0; i < s.Length / duzina; i++)
			{
				int x = s.IndexOf('\n', 0, range);
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
	}
}
