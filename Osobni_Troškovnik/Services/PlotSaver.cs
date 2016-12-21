using System;
using Gdk;
using OxyPlot;
using System.IO;
using OxyPlot.GtkSharp;
namespace Osobni_Troškovnik
{
	public abstract class PlotSaver
	{

		public static void saveToFile( string ime, PlotModel model)
		{

			var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var fullName = Path.Combine(desktopFolder, ime);

			var fs = new FileStream(fullName, FileMode.Create);
			var size = Screen.Default;

			var pngExporter = new PngExporter {Width =size.Width,Height = size.Height,Background = OxyColors.White };
			pngExporter.Export(model, fs);
			fs.Close();

		}
	}
}
