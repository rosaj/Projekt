using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.GtkSharp;
using Gtk;
using OxyPlot.Axes;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public class GrafPresenter
	{
		

		public VBox PieViewSveKategorije(DateTime odDatum, DateTime doDatum) 
		{
			
			var myModel = new PlotModel { Title = "Statistika: " +odDatum.ToString("dd.MM.yyyy") + " - " + doDatum.ToString("dd.MM.yyyy") };
			var series = new PieSeries
			{ StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

			foreach (KeyValuePair<string, double> s in Baza.getInstance.getSumiraneTroskoveURazdoblju(odDatum, doDatum))
			{
				series.Slices.Add(new PieSlice(s.Key, s.Value));

			}
			myModel.Series.Add(series);

			return dodajOstalo(myModel,odDatum,doDatum,"PieChart");
		}


		public VBox PieViewMjeseceUGodini(int godina)
		{

			var myModel = new PlotModel { Title = "Statistika za godinu: " +godina};
			var series = new PieSeries
			{ StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

			foreach (KeyValuePair<int, double> s in Baza.getInstance.getTroskovePoKategorijamaUGodini(godina))
			{
				series.Slices.Add(new PieSlice(StringManipulator.convertToMjesec(s.Key), s.Value));

			}
			myModel.Series.Add(series);

			return dodajOstalo(myModel, new DateTime(godina,1,1),new DateTime(godina+1,1,1), "GodisnjiPieChart");
		}


		public VBox BarPlotKategorija(DateTime odDatum, DateTime doDatum, string kategorija)
		{
			var items = new List<ColumnItem>();
			var datums = new List<string>();
			var list = Baza.getInstance.getGrupiraneTroskoveURazdoblju(odDatum, doDatum, KategorijaPresenter.getKategorija(kategorija));

			foreach (Trosak t in list)
			{
				var b = new ColumnItem(t.Cijena);
				items.Add(b);
				datums.Add(t.Datum.ToString("dd.MM.yyyy"));
			}

			if (list.Count > 1)
			{
				int i = 0;
				var listaBoja = OxyPalettes.Cool(list.Count).Colors;
				foreach (ColumnItem cI in items)
				{

					cI.Color = listaBoja[i];
					i++;
				}
			}


			var barSeries = new ColumnSeries()
			{
				ItemsSource = items,
				LabelPlacement = LabelPlacement.Base,
				LabelFormatString = "{0:.00} kn"
			};


			var model = new PlotModel { Title = "Statistika za: " + kategorija +" "+ odDatum.ToString("dd.MM.yyyy") + " - " + doDatum.ToString("dd.MM.yyyy")  };


			model.Series.Add(barSeries);

			model.Axes.Add(new CategoryAxis
			{
				Position = AxisPosition.Bottom,
				Key = "Datum",
				ItemsSource = datums

			});

			model.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left,
				Minimum = 0,
				LabelFormatter = StringManipulator.formatter
			});

			return dodajOstalo(model, odDatum, doDatum, "Bar");

		}

		public VBox BarPlotSveKategorije(DateTime odDatum, DateTime doDatum)
		{
			var items = new List<ColumnItem>();
			var kat = new List<string>();
			var list = Baza.getInstance.getSumiraneTroskoveURazdoblju(odDatum, doDatum);

			foreach (KeyValuePair<string, double> t in list)
			{

				var b = new ColumnItem(t.Value);
				items.Add(b);
				kat.Add(t.Key);
			}

			if (list.Count > 1)
			{
				int i = 0;
				var listaBoja = OxyPalettes.Cool(list.Count).Colors;
				foreach (ColumnItem cI in items)
				{

					cI.Color = listaBoja[i];
					i++;
				}
			}


			var barSeries = new ColumnSeries()
			{
				ItemsSource = items,
				LabelPlacement = LabelPlacement.Base,
				LabelFormatString = "{0:.00} kn"

			};


			var model = new PlotModel { Title = "Statistika za razdoblje: " + odDatum.ToString("dd.MM.yyyy") + " - " + doDatum.ToString("dd.MM.yyyy") };

			model.Series.Add(barSeries);

			model.Axes.Add(new CategoryAxis
			{
				Position = AxisPosition.Bottom,
				Key = "Datum",
				ItemsSource = kat

			});

			model.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left,
				Minimum = 0,
				LabelFormatter = StringManipulator.formatter
			});

			return dodajOstalo(model, odDatum, doDatum, "BarChartKategorije");

		}


		public VBox BarPlotMjeseceUGodini(int godina)
		{
			var items = new List<ColumnItem>();
			var kat = new List<string>();
			var list = Baza.getInstance.getTroskovePoKategorijamaUGodini(godina);

			foreach (KeyValuePair<int, double> t in list)
			{

				var b = new ColumnItem(t.Value);
				items.Add(b);
				kat.Add(StringManipulator.convertToMjesec(t.Key));
			}

			if (list.Count > 1)
			{
				int i = 0;
				var listaBoja = OxyPalettes.Cool(list.Count).Colors;
				foreach (ColumnItem cI in items)
				{

					cI.Color = listaBoja[i];
					i++;
				}
			}


			var barSeries = new ColumnSeries()
			{
				ItemsSource = items,
				LabelPlacement = LabelPlacement.Base,
				LabelFormatString = "{0:.00} kn"

			};


			var model = new PlotModel { Title = "Statistika za godinu: "+ godina };

			model.Series.Add(barSeries);

			model.Axes.Add(new CategoryAxis
			{
				Position = AxisPosition.Bottom,
				Key = "Datum",
				ItemsSource = kat

			});

			model.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left,
				Minimum = 0,
				LabelFormatter = StringManipulator.formatter
			});

			return dodajOstalo(model, new DateTime(godina,1,1), new DateTime(godina+1,1,1), "GodisnjiBarChart");

		}




		public VBox PlotSveKategorije(DateTime odDatum, DateTime doDatum)
		{
			var myModel = new PlotModel { Title = "Statistika za razdoblje: "+odDatum.ToString("dd.MM.yyyy") + " - " + doDatum.ToString("dd.MM.yyyy"), LegendTitle = "Legenda" };
			var x = new DateTimeAxis() { Title = "Datum", TitleColor = OxyColors.DarkGreen };
			var y = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, LabelFormatter = StringManipulator.formatter, Title = "Troskovi" };
			myModel.Axes.Add(x);
			myModel.Axes.Add(y);

			var lista = Kategorija.kategorije;

			foreach (var s in lista)
			{
				var l = new LineSeries() { Title = s.Naziv };
				var list = Baza.getInstance.getGrupiraneTroskoveURazdoblju(odDatum, doDatum, s);
				if (list.Count > 0)
				{
					foreach (Trosak t in list)
					{

						var datum = DateTime.ParseExact(t.Datum.ToString("dd.MM.yyyy"), "dd.MM.yyyy", null);

						l.Points.Add(new DataPoint(DateTimeAxis.ToDouble(datum), t.Cijena));

					}

					myModel.Series.Add(l);
				}
			}


			return dodajOstalo(myModel, odDatum, doDatum, "LineChartKategorije");
		}

		public VBox PlotSveKategorijePoMjesecima(int godina)
		{
			var myModel = new PlotModel { Title = "Statistika za: "+ godina+". godinu", LegendTitle = "Legenda" };
		//	var x = new DateTimeAxis() { Title = "Datum", TitleColor = OxyColors.AliceBlue };
			var y = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, LabelFormatter = StringManipulator.formatter, Title = "Troskovi" };
		//	myModel.Axes.Add(x);
			myModel.Axes.Add(y);
			var x = new LinearAxis() {Position= AxisPosition.Bottom,LabelFormatter=StringManipulator.convertToMjesec,Minimum = 1, Maximum = 12 };
			myModel.Axes.Add(x);
			var lista = Kategorija.kategorije;

			foreach (var s in lista)
			{
				var l = new LineSeries() { Title = s.Naziv };
				var list = Baza.getInstance.getTroskovePoMjesecima(s, godina);
				if (list.Count > 0)
				{
					foreach (var t in list)
					{
						l.Points.Add(new DataPoint(t.Key, t.Value));
					}

					myModel.Series.Add(l);
				}
			}


			return dodajOstalo(myModel, new DateTime(godina,1,1), new DateTime(godina+1,1,1), "LineChartKategorijeMjesecno");
		}


		public VBox PlotKategoriju(DateTime odDatum, DateTime doDatum, string kategorija)
		{
			var myModel = new PlotModel { Title = "Statistika za: " + kategorija+ " "+ odDatum.ToString("dd.MM.yyyy") + " - " + doDatum.ToString("dd.MM.yyyy"), LegendTitle = "Legenda" };
			var x = new DateTimeAxis() { Title = "Datum", TitleColor = OxyColors.DarkGreen };
			var y = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, LabelFormatter = StringManipulator.formatter, Title = "Troskovi" };
			myModel.Axes.Add(x);
			myModel.Axes.Add(y);

			var list = Baza.getInstance.getGrupiraneTroskoveURazdoblju(odDatum, doDatum,KategorijaPresenter.getKategorija( kategorija));
			var l = new LineSeries() { Title = kategorija };
			foreach (Trosak t in list)
			{
				
				DateTime datum;
				datum = DateTime.ParseExact(t.Datum.ToString("dd.MM.yyyy"), "dd.MM.yyyy", null);

				l.Points.Add(new DataPoint(DateTimeAxis.ToDouble(datum), t.Cijena));

			}

			myModel.Series.Add(l);


			return dodajOstalo(myModel, odDatum, doDatum, "LineChart");
		}



		private VBox dodajOstalo(PlotModel myModel, DateTime odDatum, DateTime doDatum, string ime)
		{
			var pv = new PlotView();
			pv.Model = myModel;
			var v = new VBox();
			v.Add(pv);
			var save = new Button(ImageButton.imageButton("gtk-save"));
			save.Clicked += (sender, e) =>
			{
				PlotSaver.saveToFile( ime+"_" + odDatum.ToString("dd.MM.yyyy") + "_-_" + doDatum.ToString("dd.MM.yyyy") + ".png", myModel);
				MessageBox.Show(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Grafikon spremljen na radnu površinu");

			};

			v.PackEnd(save, false, false, 10);
			return v;
		}


	}
}
