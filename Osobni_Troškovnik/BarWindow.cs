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
		public BarWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Stupčasti grafikon";
			Console.WriteLine("");
		}


		public void barPlotKategorija(DateTime odDatum, DateTime doDatum, string kategorija)
		{
			var items = new List<ColumnItem>();
			var datums = new List<string>();
			var list = Baza.getInstance.getGrupiraneTroskoveURazdoblju(odDatum, doDatum, kategorija);

			foreach (Trosak t in list)
			{

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
			var save = new Button(ImageButton.imageButton("gtk-save"));
			save.Clicked += (sender, e) =>
			{
				PlotSaver.saveToFile(this, "BarChart_" + kategorija + " " + odDatum.ToShortDateString() + "_-_" + doDatum.ToShortDateString() + ".png", model);
			};

			v.PackStart(save, false, false, 10);
			this.Add(v);
			this.ShowAll();

		}



		public void barPlot(DateTime odDatum, DateTime doDatum)
		{
			var items = new List<ColumnItem>();
			var kat = new List<string>();
			var list = Baza.getInstance.getSumiraneTroskoveURazdoblju(odDatum, doDatum);

			foreach (KeyValuePair<string, float> t in list)
			{

				var b = new ColumnItem(t.Value);
				items.Add(b);
				kat.Add(t.Key);
			}


			var barSeries = new ColumnSeries()
			{
				ItemsSource = items,
				LabelPlacement = LabelPlacement.Inside,
				LabelFormatString = "{0:.00} kn"

			};


			var model = new PlotModel { Title = "Statistika za razdoblje: " + odDatum.ToShortDateString() + " - " + doDatum.ToShortDateString() };
			this.SetSizeRequest(800, 600);
			var pv = new PlotView();

			model.Series.Add(barSeries);




			model.Axes.Add(new CategoryAxis
			{
				Position = AxisPosition.Bottom,
				Key = "Datum",
				ItemsSource = kat

			});

			pv.Model = model;
			var v = new VBox();
			v.Add(pv);
			var save = new Button(ImageButton.imageButton("gtk-save"));
			save.Clicked += (sender, e) =>
			{
				PlotSaver.saveToFile(this, "BarChart_" + odDatum.ToShortDateString() + "_-_" + doDatum.ToShortDateString() + ".png", model);
			};

			v.PackStart(save, false, false, 10);
			this.Add(v);
			this.ShowAll();

		}

		protected void KeyPress(object o, KeyReleaseEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;
			if (keyCode == 65307) this.Destroy();
		}
	}
}
