﻿using System;
using Gtk;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Osobni_Troškovnik
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			Gtk.Settings.Default.SetLongProperty("gtk-button-images", 1, "");
			MainWindow win = new MainWindow();


			win.Show();

			Application.Run();
		}
	}
}
