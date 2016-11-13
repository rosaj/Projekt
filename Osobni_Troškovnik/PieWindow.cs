﻿using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.GtkSharp;
using Gtk;
using System.IO;
namespace Osobni_Troškovnik
{
	public partial class PieWindow : Gtk.Window
	{
		public PieWindow(DateTime odDatum, DateTime doDatum) :base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Grafikon pita";

			this.SetSizeRequest(800, 600);
			var pv = new PlotView();
			var myModel = new PlotModel { Title = "Statistika: "+odDatum.ToShortDateString()+" - "+doDatum.ToShortDateString() };
			var series = new PieSeries
			{ StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

			foreach (string s in Baza.getInstance.getKategorije())
			{
				float suma = Baza.getInstance.getSumuTroskovaURazdoblju(odDatum, doDatum, s);
				if(suma>0)	series.Slices.Add(new PieSlice(s, suma));
					
			}
			myModel.Series.Add(series);
			pv.Model = myModel;
			var v = new VBox();
			v.Add(pv);
			var save = new Button(ImageButton.imageButton("gtk-save"));
			save.Clicked += (sender, e) => 
			{
				PlotSaver.saveToFile(this, "PieChart_" + odDatum.ToShortDateString() + "_-_" + doDatum.ToShortDateString() + ".png", myModel);
			};

			v.PackStart(save, false, false, 10);
			this.Add(v);
			this.ShowAll();

		}
	}
}
