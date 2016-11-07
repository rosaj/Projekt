using System;
using Gtk;

namespace Osobni_Troškovnik
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			MainWindow win = new MainWindow();
			win.Maximize();
			win.Show();
			Application.Run();
		}
	}
}
