using System;
using Gtk;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			MainWindow win = new MainWindow();
			Console.Write("nesto");



			Baza.getInstance.ispis();
			win.SetSizeRequest(400, 400);
			win.Show();
			Application.Run();
		}
	}
}
