using System;
using Gtk;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		private UnesiTrosakWindow uT;
		private DatumChooseWindow dCW;
		private string textF12 = "Tw Cen MT Condensed 12";
		private string textF14= "Tw Cen MT Condensed 14";
		private string defText12 = "Kristen ITC 12";
		private string defText14 = "Kristen ITC 14";
		private Gdk.Color bgColor = Props.bgColor;
		private Gdk.Color bojaSlova = Props.getColor("#0017FF");

		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			notebook.CurrentPage = 0;
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Osobni troškovnik";
			eventboxHome.ModifyBg(StateType.Normal, bgColor);
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
			
			OnDeleteEvent(sender, null);
		}
		protected void keyPressEvent(object o, KeyPressEventArgs args)
		{
			uint keyCode = args.Event.KeyValue;

		}


		private void generirajKategorije()
		{
			var lista = Baza.getInstance.getKategorije();
			var sW = new ScrolledWindow();
			var t = new Table((uint)(lista.Count + 2), 1, true);
			var eventBox = new EventBox();
			eventBox.Add(t);
			eventBox.ModifyBg(StateType.Normal, bgColor);

			sW.AddWithViewport(eventBox);
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

			var eventBox = new EventBox();
			eventBox.Add(t);
			eventBox.ModifyBg(StateType.Normal, bgColor);

			sW.AddWithViewport(eventBox);
			//sW.AddWithViewport(t);
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

			var katBoja = Props.add2EventBox(new Label(ime),bgColor, bojaSlova, textF14);
			hbox.PackStart(katBoja, false, true, 0);
			t.Attach(hbox, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

			var cijenaLab = new Label("Ukupan trošak: ");
			cijenaLab.SetAlignment(0.8f, 0.5f);
			var cijenaBoja = Props.add2EventBox(cijenaLab,bgColor,bojaSlova, defText14);
			t.Attach(cijenaBoja, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);


		//	var rangeButton = new Button();
		//	rangeButton.Label = "Filtriraj po datumu";
			w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.filterDatum.png");
			var rangeButton = new Button(w3);


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
			var opisBoja = Props.add2EventBox(lab,bgColor,bojaSlova, defText14);
			t.Attach(opisBoja, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0.1f, 0.5f);

			var lab1 = new Label("Datum");
			var datumBoja = Props.add2EventBox(lab1,bgColor,bojaSlova, defText14);
			t.Attach(datumBoja, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

			var lab2 = new Label("Cijena");
			var cijenaFg = Props.add2EventBox(lab2, bgColor, bojaSlova, defText14);
			t.Attach(cijenaFg, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.9f, 0.5f);


			var date = new Label(datumPoc.ToString("dd-MM-yyyy") + " - " + datumKraj.ToString("dd-MM-yyyy"));
			var dateBoja = Props.add2EventBox(date,bgColor,bojaSlova, defText14);

			t.Attach(dateBoja, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);


			float cijena = 0;
			Gdk.Color picked;
			for (int i = 3; i < lista.Count + 3; i++)
			{
				var trosak = lista[i - 3];

				var l = new Label(trosak.Opis);
				var l1 = new Label(trosak.Datum);
				var l2 = new Label(trosak.Cijena+" kn");
				l.SetAlignment(0.1f, 0.5f);
				//l1.SetAlignment(0.7f, 0.5f);
				l2.SetAlignment(0.9f, 0.5f);

				picked = (i % 2 == 0 ? Props.getColor("#B3BBFF") : Props.getColor("#CCD8E0"));


				var e = Props.add2EventBox(l, picked,bojaSlova, textF12);
				var e1 = Props.add2EventBox(l1, picked,bojaSlova, textF12);
				var e2 = Props.add2EventBox(l2, picked,bojaSlova, textF12);

				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				cijena += trosak.Cijena;
			}
			cijenaLab.Text += cijena+" kn";
			t.BorderWidth = 20;
			notebook.ShowAll();
			notebook.CurrentPage = 3;

		}



		private void addTotalTroskove(Dictionary<string,float> lista, DateTime datumPoc, DateTime datumKraj)
		{
			var sW = new ScrolledWindow();
			var t = new Table((uint)lista.Count + 3, 3, true);

			var eventBox = new EventBox();
			eventBox.Add(t);
			eventBox.ModifyBg(StateType.Normal, bgColor);

			sW.AddWithViewport(eventBox);

			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);
			this.SetSizeRequest(886, 575);
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
			var cijenaLabBoja = Props.add2EventBox(cijenaLab,bgColor,bojaSlova, defText14);
			t.Attach(cijenaLabBoja, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			cijenaLab.SetAlignment(0.8f, 0.5f);

		//	var rangeButton = new Button();
		//	rangeButton.Label = "Filtriraj po datumu";

			w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.filterDatum.png");
			var rangeButton = new Button(w3);

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
			var labBoja = Props.add2EventBox(lab,bgColor, bojaSlova, defText14);
			t.Attach(labBoja, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0.2f, 0.5f);
			var lab2 = new Label("Ukupna cijena");
			var lab2Boja = Props.add2EventBox(lab2,bgColor,bojaSlova, defText14);
			t.Attach(lab2Boja, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.8f, 0.5f);

			var date = new Label(datumPoc.ToString("dd-MM-yyyy") + " - " + datumKraj.ToString("dd-MM-yyyy"));

			var dateBoja = Props.add2EventBox(date,bgColor, bojaSlova, defText14);
			t.Attach(dateBoja, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);


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
				var l2 = new Label((total)+" kn");
				l.SetAlignment(0.2f, 0.5f);

				l2.SetAlignment(0.8f, 0.5f);

				picked = (i % 2 == 0 ? Props.getColor("#0099D4") : Props.getColor("#FFFFFF"));
				var slovoPi = (i % 2 == 0 ? Props.getColor("#FFFFFF"): bojaSlova);

				// # FCF1FF

				var e = Props.add2EventBox(l, picked,slovoPi, textF14);
				var e1 = Props.add2EventBox(new Label(), picked);
				var e2 = Props.add2EventBox(l2, picked, slovoPi, textF14);

				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				cijena += total;
				i++;
			}
			cijenaLab.Text += cijena+" kn";
			t.BorderWidth = 50;
			notebook.ShowAll();
			notebook.CurrentPage = 2;


		}

		private void addStatisticView(List<string> lista, DateTime odDatum, DateTime doDatum)
		{

			var sW = new ScrolledWindow();

			int iter = (lista.Count) / 3;
			if ((lista.Count) % 3 != 0) iter++;


			var t = new Table((uint)iter + 3, 3, true);

			var eventBox = new EventBox();
			eventBox.Add(t);
			eventBox.ModifyBg(StateType.Normal, bgColor);

			sW.AddWithViewport(eventBox);

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

		//	var rangeButton = new Button();
		//	rangeButton.Label = "Filtriraj po datumu";
			w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.filterDatum.png");
			var rangeButton = new Button(w3);

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

			var hbox = new HBox(false, 20);

			hbox.PackEnd(new Fixed(), false, false, 0);
			hbox.PackEnd(bar, false, false, 0);
			hbox.PackEnd(lin, false, false, 0);
			hbox.PackEnd(pie, false, false, 0);

			t.Attach(hbox, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);



			var lab = new Label("Kategorije");
			var labBoja = Props.add2EventBox(lab,bgColor, bojaSlova,defText14);
			t.Attach(labBoja, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			//lab.SetAlignment(0, 0.5f);

			var date = new Label(odDatum.ToString("dd-MM-yyyy") + " - " + doDatum.ToString("dd-MM-yyyy"));
			var dateBoja = Props.add2EventBox(date,bgColor,bojaSlova, defText14);
			t.Attach(dateBoja, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

			/*	Gdk.Color picked;

				for (int i = 3; i < lista.Count + 3; i++)
				{
					var kategorija = lista[i - 3];

					var l = new Label(kategorija);

					l.SetAlignment(0.2f, 0.5f);


					//picked = (i % 2 == 0 ? Props.getColor("#A5BEFF") : Props.getColor("#D7D7D7"));
					picked = (i % 2 == 0 ? Props.getColor("#0099D4") : Props.getColor("#FFFFFF"));
					var slovoPi = (i % 2 == 0 ? Props.getColor("#FFFFFF") : bojaSlova);


					var e = Props.add2EventBox(l, picked,slovoPi, textF14);
					var e1 = Props.add2EventBox(new Label(), picked);
					var b1 = new Button(ImageButton.imageButton("Line"));
					var b2 = new Button(ImageButton.imageButton("Bar"));
					var hb = new HBox(false, 20);
					hb.PackEnd(b2, false, false, 0);
					hb.PackEnd(b1, false, false, 0);

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

				*/




			int br = 0;
			for (int i = 0; i < iter; i++)
			{


				for (int y = 0; y < 3 && br < lista.Count; y++)
				{
					var kategorija = lista[br];
					br++;
					var l = new Label(kategorija);

					//l.SetAlignment(0.2f, 0.5f);

				//var e = Props.add2EventBox(l,Props.getColor("white"));
					var b1 = new Button(ImageButton.imageButton("Line"));
					var b2 = new Button(ImageButton.imageButton("Bar"));
					var hb = new HBox(false, 20);
					hb.PackStart(new Fixed());
					hb.PackStart(b1,false, false, 0);
					hb.PackStart(b2, false,false,0);
					hb.PackStart(new Fixed());
				

					var vbox = new VBox(false, 5);
					vbox.Add(Props.add2EventBox(l,Props.getColor("#0099D4"),Props.getColor("White"),textF12));
					vbox.PackEnd(hb, false, false, 0);
					var e2 = Props.add2EventBox(vbox, Props.getColor("#0099D4"));

					t.Attach(e2, (uint)(y),(uint) (y+1), (uint)i+3, (uint)(i + 4), AttachOptions.Fill, AttachOptions.Fill, 20, 0);

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


			}


			t.BorderWidth = 50;
			notebook.ShowAll();
			notebook.CurrentPage = 2;


		}


	}
}
