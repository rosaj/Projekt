using System;
using Gtk;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		private UnesiTrosakWindow uT;
		private DatumChooseWindow dCW;

	//	private string textF12 = "Sans Condensed Not-Rotated 12";
	//	private string textF14 = "Sans Condensed Not-Rotated 14";
		//private string defText12 = "Kristen ITC 12";
	//	private string defText14 = "Kristen ITC 14";
		private Gdk.Color bgColor = Props.bgColor;
		private Gdk.Color bojaSlova = Props.getColor("#0017FF");

		private DateTime p = DateTime.Now.AddMonths(-1);
		private DateTime k = DateTime.Now;

		private string currentKategorija;

		TrosakNodeStore trosakPresenter;

		private EventHandler datumChanged;
		private EventHandler backClicked;

		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			notebook.CurrentPage = 0;
			this.Icon = this.RenderIcon("Icon", IconSize.Menu, null);
			this.Title = "Osobni troškovnik";
			eventboxHome.ModifyBg(StateType.Normal, bgColor);
			eventBoxTroskovi.ModifyBg(StateType.Normal, bgColor);
			eventBoxStatistika.ModifyBg(StateType.Normal, bgColor);

			trosakPresenter = new TrosakNodeStore();

			setupTreeView();
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
			generirajKategorije();
		}

		protected void totalCostClicked(object sender, EventArgs e)
		{
			addTotalTroskove(Baza.getInstance.getSumiraneTroskoveURazdoblju(p, k), p, k);
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
			if (keyCode == 65480) this.Fullscreen();
			else if (keyCode == 65307) this.Iconify();
		}


		protected void editTrosakClicked(object sender, EventArgs e)
		{

			var selectedTrosak = (TrosakNode)nodeView.NodeSelection.SelectedNode;
			if (selectedTrosak != null)
			{
				editTrosak(selectedTrosak);
			}
		}

		private void editTrosak(TrosakNode tn)
		{
			var editWin = new EditTrosakWindow(tn.trosak);
			var t = tn.trosak;
			editWin.signal += (sender1, e1) =>
			{
				tn.cijena = t.Cijena.ToString("0.00 kn");
				tn.datum = t.Datum;
				tn.opis = t.Opis;
				opisView.Buffer.Text = t.Opis;
			};
			editWin.brisiTrosak += (sender2, e2) =>
			  {
				  nodeView.NodeStore.RemoveNode(tn);
			  };
			
		}
			
		protected void backButtonClicked(object sender, EventArgs e)
		{
			datumChanged = null;

			notebook.CurrentPage = 0;
			if(backClicked != null)	backClicked(sender, e);
			backClicked = null;
		}

		protected void datumFilterClicked(object sender, EventArgs e)
		{
			
				dCW = new DatumChooseWindow(p,k);
				dCW.signaliziraj += (odDatum, doDatum) =>
				{
					p = odDatum;
					k = doDatum;
				if (datumChanged != null)datumChanged(sender, e);
				};
		}
		private void prikaziPodatke(string kategorija)
		{
				currentKategorija = kategorija;
				
				trosakPresenter.brisiTroskove();
				trosakPresenter.dodajTroskoveURazdoblju(kategorija, p, k);
				//trosakPresenter.dodaj(listaTr);
				nodeView.NodeStore = trosakPresenter;
				kategorijaLabel.LabelProp = currentKategorija;
				infoUkupno.LabelProp = trosakPresenter.Suma;
				infoProsjek.LabelProp = trosakPresenter.Prosjek;
		}

		public void setupTreeView()
		{
			var cijenaColumn = new TreeViewColumn("Cijena", new CellRendererText(), "text", 1);
			var datumColumn = new TreeViewColumn("Datum", new CellRendererText(), "text", 0);

			cijenaColumn.Clickable = true;
			cijenaColumn.Reorderable = true;
			cijenaColumn.SortIndicator = true;
			cijenaColumn.Resizable = true;
			cijenaColumn.MinWidth = 100;
			cijenaColumn.Clicked += (sender, e) => sortiranjePoCijeni(cijenaColumn);

			datumColumn.Clickable = true;
			datumColumn.Reorderable = true;
			datumColumn.SortIndicator = true;
			datumColumn.Resizable = true;
			datumColumn.SortOrder = SortType.Descending;
			datumColumn.MinWidth = 100;
			datumColumn.Clicked += (sender, e) => sortiranjePoDatumu(datumColumn);

			nodeView.AppendColumn(cijenaColumn);
			nodeView.AppendColumn(datumColumn);
		}

		private void sortiranjePoCijeni(TreeViewColumn cijenaCol)
		{
			if (cijenaCol.SortOrder == SortType.Descending)
			{
				trosakPresenter.sortByCijena(SortType.Ascending);
				cijenaCol.SortOrder = SortType.Ascending;
			}
			else 
			{
				trosakPresenter.sortByCijena(SortType.Descending);
				cijenaCol.SortOrder = SortType.Descending;
			}
		}

		private void sortiranjePoDatumu(TreeViewColumn datumCol)
		{
			if (datumCol.SortOrder == SortType.Descending)
			{
				trosakPresenter.sortByDatum(SortType.Ascending);
				datumCol.SortOrder = SortType.Ascending;
			}
			else
			{
				trosakPresenter.sortByDatum(SortType.Descending);
				datumCol.SortOrder = SortType.Descending;
			}
			
		}
		private void generirajKategorije()
		{
			notebook.CurrentPage = 1;

			var kategorijeBPresenter = new KategorijeButtonPresenter();
			var vBoxKat = kategorijeBPresenter.Kategorije((Widget w) => 
			{
				var b = w as Button;
				b.Clicked += (sender, e) => prikaziPodatke(b.Name);

			});
			kategorijeHBox.Add(vBoxKat);
			refreshPodatke();

			nodeView.NodeSelection.Changed += (sender, e) =>
			{
				var selectedTrosak = (TrosakNode)nodeView.NodeSelection.SelectedNode;
				if (selectedTrosak != null)
				{
					opisView.Buffer.Text = selectedTrosak.opis;
				}

				else opisView.Buffer.Text = "";
			};

			datumChanged += (sender, e) => refreshPodatke();

			backClicked+=(sender, e) => troskoviBackClicked();
		
			notebook.ShowAll();
		}
		private void refreshPodatke()
		{
			if (currentKategorija != null) prikaziPodatke(currentKategorija);
			datumLabel.LabelProp = p.ToString("dd-MM-yyyy") + " - " + k.ToString("dd-MM-yyyy");
		}
		private void troskoviBackClicked()
		{
			infoUkupno.LabelProp = "";
			infoProsjek.LabelProp = "";
			kategorijaLabel.LabelProp = "";
			var list = kategorijeHBox.AllChildren;
			foreach (Widget w in list)
			{
				kategorijeHBox.Remove(w);
			}
			trosakPresenter.brisiTroskove();
			if (nodeView.NodeStore != null) nodeView.NodeStore.Clear();

		}
		private void addTotalTroskove(Dictionary<string,double> lista, DateTime datumPoc, DateTime datumKraj)
		{
			/*var sW = new ScrolledWindow();
			var t = new Table((uint)lista.Count + 3, 3, true);

			var eventBox = new EventBox();
			eventBox.Add(t);
			eventBox.ModifyBg(StateType.Normal, bgColor);

			sW.AddWithViewport(eventBox);

			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;
			t.WidthRequest = 800;


			var cijenaLab = new Label("Ukupno: ");
			var cijenaLabBoja = Props.add2EventBox(cijenaLab, bgColor, bojaSlova, defText14);
			t.Attach(cijenaLabBoja, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			cijenaLab.SetAlignment(0.8f, 0.5f);

		var w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.filterDatum.png");
			var rangeButton = new Button(w3);

			rangeButton.Clicked += (sender, e) =>
			{

				dCW = new DatumChooseWindow(p,k);
				dCW.signaliziraj += (odDatum, doDatum) =>
				{
					notebook.Remove(sW);
					addTotalTroskove(Baza.getInstance.getSumiraneTroskoveURazdoblju(odDatum, doDatum), odDatum, doDatum);
				};
			};

			w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.back.png");
			var back = new Button(w3);

			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 0;
			};


			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);
			t.Attach(rangeButton, 1, 2, 1, 2, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);

			var lab = new Label("Kategorija");
			var labBoja = Props.add2EventBox(lab, bgColor, bojaSlova, defText14);
			t.Attach(labBoja, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0.2f, 0.5f);
			var lab2 = new Label("Ukupna cijena");
			var lab2Boja = Props.add2EventBox(lab2, bgColor, bojaSlova, defText14);
			t.Attach(lab2Boja, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.8f, 0.5f);

			var date = new Label(datumPoc.ToString("dd-MM-yyyy") + " - " + datumKraj.ToString("dd-MM-yyyy"));

			var dateBoja = Props.add2EventBox(date, bgColor, bojaSlova, defText14);
			t.Attach(dateBoja, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

			Gdk.Color picked;
			double cijena = 0;
		
			int i = 3;
			foreach (KeyValuePair<string, double> dic in lista)
			{
				var kategorija = dic.Key;

				var l = new Label(kategorija);

				double total = dic.Value;
				var l2 = new Label(total.ToString("N") + " kn");
				l.SetAlignment(0.2f, 0.5f);

				l2.SetAlignment(0.8f, 0.5f);

				picked = (i % 2 == 0 ? Props.getColor("#0099D4") : Props.getColor("#FFFFFF"));
				var slovoPi = (i % 2 == 0 ? Props.getColor("#FFFFFF") : bojaSlova);

				// # FCF1FF

				var e = Props.add2EventBox(l, picked, slovoPi, textF14);
				var e1 = Props.add2EventBox(new Label(), picked);
				var e2 = Props.add2EventBox(l2, picked, slovoPi, textF14);

				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				cijena += total;
				i++;
			}
			cijenaLab.Text += cijena.ToString("N") + " kn";
			t.BorderWidth = 50;
			notebook.ShowAll();
			notebook.CurrentPage = 1;

*/
		}

		private void addStatisticView(List<string> lista, DateTime odDatum, DateTime doDatum)
		{
			/*
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


			//	var rangeButton = new Button();
			//	rangeButton.Label = "Filtriraj po datumu";
			var w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.filterDatum.png");
			var rangeButton = new Button(w3);

			rangeButton.Clicked += (sender, e) =>
			{
				dCW = new DatumChooseWindow(p,k);
				dCW.signaliziraj += (odDat, doDat) =>
				{
					notebook.Remove(sW);
					addStatisticView(Baza.getInstance.getKategorije(), odDat, doDat);
				};
			};

	w3 = new Image();
			w3.Pixbuf = Gdk.Pixbuf.LoadFromResource("Osobni_Troškovnik.Pics.back.png");
			var back = new Button(w3);

			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);

			//back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 0;

			};

			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);
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
			var labBoja = Props.add2EventBox(lab, bgColor, bojaSlova, defText14);
			t.Attach(labBoja, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			//lab.SetAlignment(0, 0.5f);

			var date = new Label(odDatum.ToString("dd-MM-yyyy") + " - " + doDatum.ToString("dd-MM-yyyy"));
			var dateBoja = Props.add2EventBox(date, bgColor, bojaSlova, defText14);
			t.Attach(dateBoja, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

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
					hb.PackStart(b1, false, false, 0);
					hb.PackStart(b2, false, false, 0);
					hb.PackStart(new Fixed());


					var vbox = new VBox(false, 5);
					vbox.Add(Props.add2EventBox(l, Props.getColor("#0099D4"), Props.getColor("White"), textF12));
					vbox.PackEnd(hb, false, false, 0);
					var e2 = Props.add2EventBox(vbox, Props.getColor("#0099D4"));

					t.Attach(e2, (uint)(y), (uint)(y + 1), (uint)i + 3, (uint)(i + 4), AttachOptions.Fill, AttachOptions.Fill, 20, 0);

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
			notebook.CurrentPage = 1;

*/
			notebook.CurrentPage = 2;
			totalBar.Image =  new Image("Bar", IconSize.Dnd);
			totalPie.Image = new Image("Pie", IconSize.Dnd);
			totalLine.Image = new Image("Line", IconSize.Dnd);
			kategorijaBar.Image = new Image("Bar", IconSize.Dnd);
			kategorijaLine.Image = new Image("Line", IconSize.Dnd);
			prikazPoBar.Image = new Image("Bar", IconSize.Dnd);
			prikazPoPie.Image = new Image("Pie", IconSize.Dnd);
			odabranaGodina.Text = DateTime.Now.Year.ToString();
			var kP = new KategorijaPresenter(kategorijeCombo);
			datumLabela.LabelProp = p.ToString("dd.MM.yyyy") + " - " + k.ToString("dd.MM.yyyy");

		}

		protected void totalClicked(object sender, EventArgs e)
		{
			pocistiPlotBox();
			var b = sender as Button;

			if (b.Name == totalBar.Name)
			{
				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.BarPlotSveKategorije(p, k));
				plotBox.ShowAll();
			}
			else if (b.Name == totalPie.Name)
			{
				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.PieViewSveKategorije(p, k));
				plotBox.ShowAll();
			}

			else if (b.Name == totalLine.Name)
			{
				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.PlotSveKategorije(p, k));
				plotBox.ShowAll();
			}
		}

		protected void kategorijaGrafClicked(object sender, EventArgs e)
		{
			pocistiPlotBox();
			var b = sender as Button;

			if (b.Name == kategorijaBar.Name)
			{
				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.BarPlotKategorija(p, k, kategorijeCombo.ActiveText));
				plotBox.ShowAll();
			}
			else if (b.Name == kategorijaLine.Name)
			{
				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.PlotKategoriju(p, k,kategorijeCombo.ActiveText));
				plotBox.ShowAll();
			}
		}

		protected void prikazPoGrafClicked(object sender, EventArgs e)
		{
			pocistiPlotBox();
			var b = sender as Button;
			if (b.Name == prikazPoBar.Name)
			{
				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.BarPlotMjeseceUGodini(Int32.Parse(odabranaGodina.Text)));
				plotBox.ShowAll();
			}
			else if (b.Name == prikazPoPie.Name)
			{

				var grafPresenter = new GrafPresenter();
				plotBox.Add(grafPresenter.PieViewMjeseceUGodini(Int32.Parse(odabranaGodina.Text)));
				plotBox.ShowAll();
			}
			
		}
		private void pocistiPlotBox()
		{
			var list = plotBox.AllChildren;
			foreach (Widget w in list)
			{
				plotBox.Remove(w);
			}
		}
	}
}
