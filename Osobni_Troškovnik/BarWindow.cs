using System;
using Gtk;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class BarWindow : Gtk.Window
	{
		public BarWindow() :base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}


		public void barPlot(DateTime odDatum, DateTime doDatum, string kategorija)
		{
			var items = new List<ColumnItem>();
			var datums = new List<string>();
			var list = Baza.getInstance.getTroskoveURazdoblju(odDatum, doDatum, kategorija);

			for (int i = list.Count-1; i >= 0; --i)
			{
				var t = list[i];
				var b = new ColumnItem(t.Cijena);
				items.Add(b);
				datums.Add(t.Datum);
			}


			var barSeries = new ColumnSeries()
			{
				ItemsSource = items,
				LabelPlacement = LabelPlacement.Inside,
				LabelFormatString = "{0:.00} kn"

			};


			var model = new PlotModel { Title = "Statistika za: " + kategorija };
			this.SetSizeRequest(800, 600);
			var pv = new PlotView();

			model.Series.Add(barSeries);




			model.Axes.Add(new CategoryAxis
			{
				Position = AxisPosition.Bottom,
				Key = "Datum",
				ItemsSource = datums
				                       
			});





			pv.Model = model;
			var v = new VBox();
			v.Add(pv);
			this.Add(v);
			this.ShowAll();


		}
	}
}
