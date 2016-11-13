using System;
using Gtk;
using OxyPlot;
using System.IO;
using OxyPlot.GtkSharp;
namespace Osobni_Troškovnik
{
	public abstract class PlotSaver
	{

		public static void saveToFile(Window w,string ime, PlotModel model )
		{

			var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var fullName = System.IO.Path.Combine(desktopFolder, ime);

			Console.WriteLine(Environment.CurrentDirectory);
			var fs = new FileStream(fullName, FileMode.Create);

			var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
			pngExporter.Export(model, fs);
			fs.Close();
			MessageBox.Show(w, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Grafikon spremljen na radnu površinu");



		}
	}
}
