using System;
using Gtk;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		private UnesiTrosakWindow uT;
		private DatumChooseWindow dCW;
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			notebook.CurrentPage = 0;
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Osobni troškovnik";
		}
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Baza.getInstance.closeCon();
			Application.Quit();
			a.RetVal = true;
		}

		protected void noviTrosakClicked(object sender, EventArgs e)
		{
			if (uT == null)
			{
				uT = new UnesiTrosakWindow();
				uT.signaliziraj += () => uT = null;
			}
		}
		protected void popisClicked(object sender, EventArgs e)
		{
			notebook.CurrentPage = 1;
			generirajKategorije();
		}

		protected void totalCostClicked(object sender, EventArgs e)
		{
			var p = DateTime.Now.AddMonths(-1);
			var k = DateTime.Now;
			addTotalTroskove(Baza.getInstance.getSumiraneTroskoveURazdoblju(p,k),p,k);

		}

		protected void statistikaClicked(object sender, EventArgs e)
		{
			addStatisticView(Baza.getInstance.getKategorije(), DateTime.Now.AddMonths(-1), DateTime.Now);
		}

		protected void izlazClicked(object sender, EventArgs e)
		{
			OnDeleteEvent(sender, new DeleteEventArgs());
		}
		private void generirajKategorije()
		{
			var lista = Baza.getInstance.getKategorije();
			var sW = new ScrolledWindow();
			var t = new Table((uint)(lista.Count + 2), 1, true);
			sW.AddWithViewport(t);
			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;

			//var back = new Button(ImageButton.imageButton("gtk-go-back"));

			var w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.back.png");
			var back = new Button(w3);
			t.Attach(back, 0, 1, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);
		//	back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 0;
			};
			var sep = new HSeparator();
			t.Attach(sep, 0, 1, 1, 2, AttachOptions.Expand, AttachOptions.Fill, 0, 0);





			for (int i = 2; i <= lista.Count + 1; i++)
			{
				string s = lista[i - 2];
				string icon = s;
				if (!Props.defultLista.Contains(s))
					icon = "r";
				var b = ImageButton.imageButton(icon, s);


				b.HeightRequest = 50;
				b.WidthRequest = 400;

				t.Attach(b, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Expand, AttachOptions.Fill, 0, 0);
				b.Clicked += (sender, e) => kategorijaClicked(((Button)sender).Name);
			}

			notebook.ShowAll();
			notebook.CurrentPage = 2;
		}

		private void kategorijaClicked(string ime)
		{
			var p = DateTime.Now.AddMonths(-1);
			var k = DateTime.Now;
			addTroskove(Baza.getInstance.getTroskoveURazdoblju(p, k, ime), ime, p, k);

		}

		private void addTroskove(List<Trosak> lista, string ime, DateTime datumPoc, DateTime datumKraj)
		{


			var sW = new ScrolledWindow();
			var t = new Table((uint)lista.Count + 3, 3, true);
			sW.AddWithViewport(t);
			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;
			t.WidthRequest = 800;

		//	var back = new Button(ImageButton.imageButton("gtk-go-back"));
		
			var w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.back.png");
			var back = new Button(w3);


			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);

			//back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 2;
			};

			var kategorija = ImageButton.imageButton(ime);
			var hbox = new HBox(false, 6);
			hbox.PackStart(kategorija, false, true, 0);
			hbox.PackStart(new Label(ime), false, true, 0);
			t.Attach(hbox, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

			var cijenaLab = new Label("Ukupan trošak: ");
			t.Attach(cijenaLab, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			cijenaLab.SetAlignment(0.8f, 0.5f);

			var rangeButton = new Button();
			rangeButton.Label = "Filtriraj po datumu";
			rangeButton.Clicked += (sender, e) =>
			{
				if (dCW == null)
				{
					dCW = new DatumChooseWindow();
					dCW.signaliziraj += (odDatum, doDatum) =>
					{
						notebook.Remove(sW);
						addTroskove(Baza.getInstance.getTroskoveURazdoblju(odDatum, doDatum, ime), ime, odDatum, doDatum);
						dCW = null;
					};
					dCW.cancelOdabiranje += () => dCW = null;
				}

			};

			t.Attach(rangeButton, 1, 2, 1, 2, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);

			var lab = new Label("Opis");
			t.Attach(lab, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0.1f, 0.5f);
			var lab1 = new Label("Datum");
			t.Attach(lab1, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			//lab1.SetAlignment(0.7f, 0.5f);
			var lab2 = new Label("Cijena");
			t.Attach(lab2, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.9f, 0.5f);


			var date = new Label("Razdoblje: " + datumPoc.ToString("dd-MM-yyyy") + " - " + datumKraj.ToString("dd-MM-yyyy"));
			t.Attach(date, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);


			float cijena = 0;
			Gdk.Color picked;
			for (int i = 3; i < lista.Count + 3; i++)
			{
				var trosak = lista[i - 3];

				var l = new Label(trosak.Opis);
				var l1 = new Label(trosak.Datum);
				var l2 = new Label(trosak.Cijena.ToString());
				l.SetAlignment(0.1f, 0.5f);
				//l1.SetAlignment(0.7f, 0.5f);
				l2.SetAlignment(0.9f, 0.5f);

				picked = (i % 2 == 0 ? Props.getColor("#B3BBFF") : Props.getColor("#CCD8E0"));



				var e = Props.add2EventBox(l, picked);
				var e1 = Props.add2EventBox(l1, picked);
				var e2 = Props.add2EventBox(l2, picked);


				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				cijena += trosak.Cijena;
			}
			cijenaLab.Text += cijena.ToString();
			t.BorderWidth = 20;
			notebook.ShowAll();
			notebook.CurrentPage = 3;

		}



		private void addTotalTroskove(Dictionary<String,float> lista, DateTime datumPoc, DateTime datumKraj)
		{
			var sW = new ScrolledWindow();
			var t = new Table((uint)lista.Count + 3, 3, true);
			sW.AddWithViewport(t);
			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;
			t.WidthRequest = 800;

			//var back = new Button(ImageButton.imageButton("gtk-go-back"));
			var w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.back.png");
			var back = new Button(w3);


			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);

			//back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 0;
			};


			var cijenaLab = new Label("Ukupan trošak: ");
			t.Attach(cijenaLab, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			cijenaLab.SetAlignment(0.8f, 0.5f);

			var rangeButton = new Button();
			rangeButton.Label = "Filtriraj po datumu";
			rangeButton.Clicked += (sender, e) =>
			{
				if (dCW == null)
				{
					dCW = new DatumChooseWindow();
					dCW.signaliziraj += (odDatum, doDatum) =>
					{
						notebook.Remove(sW);
						addTotalTroskove(Baza.getInstance.getSumiraneTroskoveURazdoblju(odDatum,doDatum), odDatum, doDatum);
						dCW = null;
					};
					dCW.cancelOdabiranje += () => dCW = null;

				}

			};

			t.Attach(rangeButton, 1, 2, 1, 2, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);

			var lab = new Label("Kategorija");
			t.Attach(lab, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0.2f, 0.5f);
			var lab2 = new Label("Ukupna cijena");
			t.Attach(lab2, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.8f, 0.5f);

			var date = new Label("Razdoblje: " + datumPoc.ToString("dd-MM-yyyy") + " - " + datumKraj.ToString("dd-MM-yyyy"));
			t.Attach(date, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);


			Gdk.Color picked;
			float cijena = 0;

			//	for (int i = 3; i < lista.Count + 3; i++)
			//{
			//var kategorija = lista[i - 3];
			int i = 3;
				foreach(KeyValuePair<string,float> dic in lista)
				{
					var kategorija = dic.Key;
					
				var l = new Label(kategorija);
					//float total = Baza.getInstance.getSumuTroskovaURazdoblju(datumPoc, datumKraj, kategorija);
					float total = dic.Value;
				var l2 = new Label((total).ToString());
				l.SetAlignment(0.2f, 0.5f);

				l2.SetAlignment(0.8f, 0.5f);

				picked = (i % 2 == 0 ? Props.getColor("#C4FAFF") : Props.getColor("#FCF1FF"));



				var e = Props.add2EventBox(l, picked);
				var e1 = Props.add2EventBox(new Label(), picked);
				var e2 = Props.add2EventBox(l2, picked);


				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				cijena += total;
				i++;
			}
			cijenaLab.Text += cijena;
			t.BorderWidth = 50;
			notebook.ShowAll();
			notebook.CurrentPage = 2;


		}

		private void addStatisticView(List<string> lista, DateTime odDatum, DateTime doDatum)
		{

			var sW = new ScrolledWindow();
			var t = new Table((uint)lista.Count + 3, 3, true);
			sW.AddWithViewport(t);
			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;
			t.WidthRequest = 800;

	//		var back = new Button(ImageButton.imageButton("gtk-go-back"));
	
			var w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.back.png");
			var back = new Button(w3);

			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);

			//back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 0;
			};

			var rangeButton = new Button();
			rangeButton.Label = "Filtriraj po datumu";
			rangeButton.Clicked += (sender, e) =>
			{
				if (dCW == null)
				{
					dCW = new DatumChooseWindow();
					dCW.signaliziraj += (odDat, doDat) =>
					{
						notebook.Remove(sW);
						addStatisticView(Baza.getInstance.getKategorije(), odDat, doDat);
						dCW = null;
					};
					dCW.cancelOdabiranje += () => dCW = null;
				}

			};

			t.Attach(rangeButton, 1, 2, 1, 2, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);

			var pie = new Button(ImageButton.imageButton("Pie"));
			var lin = new Button(ImageButton.imageButton("Line"));
			var bar = new Button(ImageButton.imageButton("Bar"));
			pie.Clicked += (sender, e) =>
			{
				var pieChart = new PieWindow(odDatum, doDatum);
			};
			lin.Clicked += (sender, e) =>
			{
				var lineChart = new PlotWindow();
				lineChart.plotSveKategorije(odDatum, doDatum);
			};
			bar.Clicked += (sender, e) =>
			{
				var barChart = new BarWindow();
				barChart.barPlot(odDatum, doDatum);

			};

			var hbox = new HBox(true, 10);
			hbox.PackStart(lin, false, false, 5);
			hbox.PackEnd(pie, false, false, 5);
			hbox.PackEnd(bar, false, false, 5);
			t.Attach(hbox, 2, 3, 2, 3, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);



			var lab = new Label("Kategorija");
			t.Attach(lab, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0, 0.5f);

			var date = new Label("Razdoblje: " + odDatum.ToString("dd-MM-yyyy") + " - " + doDatum.ToString("dd-MM-yyyy"));
			t.Attach(date, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

			Gdk.Color picked;

			for (int i = 3; i < lista.Count + 3; i++)
			{
				var kategorija = lista[i - 3];

				var l = new Label(kategorija);

				l.SetAlignment(0.2f, 0.5f);


				picked = (i % 2 == 0 ? Props.getColor("#FFEAC9") : Props.getColor("#D7D7D7"));



				var e = Props.add2EventBox(l, picked);
				var e1 = Props.add2EventBox(new Label(), picked);
				var b1 = new Button(ImageButton.imageButton("Line"));
				var b2 = new Button(ImageButton.imageButton("Bar"));
				var hb = new HBox(true, 10);
				hb.PackStart(b1, false, true, 0);
				hb.PackStart(b2, false, true, 0);

				var e2 = Props.add2EventBox(hb, picked);


				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);

				b1.Clicked += (sender, ev) =>
				{
					var line = new PlotWindow();
					line.plotKategoriju(odDatum, doDatum, kategorija);
				};
				b2.Clicked += (sender, ev) =>
				{
					var barC = new BarWindow();
					barC.barPlotKategorija(odDatum, doDatum, kategorija);
				};
			}

			t.BorderWidth = 50;
			notebook.ShowAll();
			notebook.CurrentPage = 2;


		}
	}
}
