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
			win.SetSizeRequest(400, 400);
			win.Show();
			Application.Run();
		}
	}
}
