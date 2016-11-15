using System;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
using Gtk;
using OxyPlot.Axes;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class PlotWindow : Gtk.Window
	{
		public PlotWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Linijski grafikon";
		}

		public void plotSveKategorije(DateTime odDatum, DateTime doDatum)
		{
			this.SetSizeRequest(800, 500);
			var pv = new PlotView();

			var myModel = new PlotModel { Title = "Statistika", LegendTitle = "Legenda" };
			var x = new DateTimeAxis() { Title = "Datum", TitleColor = OxyColors.DarkGreen };
			var y = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, LabelFormatter = StringManipulator.formatter, Title = "Troskovi" };
			myModel.Axes.Add(x);
			myModel.Axes.Add(y);

			var lista = Baza.getInstance.getKategorije();

			foreach (string s in lista)
			{
				var l = new LineSeries() { Title = s };
				var list = Baza.getInstance.getTroskoveURazdoblju(odDatum, doDatum, s);
				if (list.Count > 0)
				{
					foreach (Trosak t in list)
					{

						DateTime datum;
						datum = DateTime.ParseExact(t.Datum, "dd-MM-yyyy", null);

						l.Points.Add(new DataPoint(DateTimeAxis.ToDouble(datum), t.Cijena));

					}

					myModel.Series.Add(l);
				}
			}


			pv.Model = myModel;
			var v = new VBox();
			v.Add(pv);
			var save = new Button(ImageButton.imageButton("gtk-save"));
			save.Clicked += (sender, e) =>
			{
				PlotSaver.saveToFile(this, "LineChart_" + odDatum.ToShortDateString() + "_-_" + doDatum.ToShortDateString() + ".png", myModel);
			};

			v.PackStart(save, false, false, 10);
			this.Add(v);
			this.ShowAll();
		}
		public void plotKategoriju(DateTime odDatum, DateTime doDatum, string kategorija)
		{
			this.SetSizeRequest(800, 500);
			var pv = new PlotView();

			var myModel = new PlotModel { Title = "Statistika za: " + kategorija, LegendTitle = "Legenda" };
			var x = new DateTimeAxis() { Title = "Datum", TitleColor = OxyColors.DarkGreen };
			var y = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, LabelFormatter = StringManipulator.formatter, Title = "Troskovi" };
			myModel.Axes.Add(x);
			myModel.Axes.Add(y);

			var list = Baza.getInstance.getTroskoveURazdoblju(odDatum, doDatum, kategorija);
			var l = new LineSeries() { Title = kategorija };
			foreach (Trosak t in list)
			{

				DateTime datum;
				datum = DateTime.ParseExact(t.Datum, "dd-MM-yyyy", null);

				l.Points.Add(new DataPoint(DateTimeAxis.ToDouble(datum), t.Cijena));

			}

			myModel.Series.Add(l);


			pv.Model = myModel;
			var v = new VBox();
			v.Add(pv);
			var save = new Button(ImageButton.imageButton("gtk-save"));
			save.Clicked += (sender, e) =>
			{
				PlotSaver.saveToFile(this, "LineChart_" + kategorija + "_" + odDatum.ToShortDateString() + "_-_" + doDatum.ToShortDateString() + ".png", myModel);
			};

			v.PackStart(save, false, false, 10);
			this.Add(v);
			this.ShowAll();
		}


	}
}
