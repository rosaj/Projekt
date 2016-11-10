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
			Gtk.Settings.Default.SetLongProperty("gtk-button-images", 1, "");
			MainWindow win = new MainWindow();

			Baza.getInstance.ispis();
			win.Show();

			Application.Run();
		}
	}
}
