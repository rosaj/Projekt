using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		


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
		GrafPresenter grafPresenter;

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
			
				var uT = new UnesiTrosakWindow(this);
			
		}
		protected void popisClicked(object sender, EventArgs e)
		{
			generirajKategorije();
		}

		protected void totalCostClicked(object sender, EventArgs e)
		{
			addTotalTroskove();
		}

		protected void statistikaClicked(object sender, EventArgs e)
		{
			addStatisticView();
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
				
				var editWin = new EditTrosakWindow(selectedTrosak.trosak,this);
				var t = selectedTrosak.trosak;
				editWin.signal += (sender1, e1) =>
				{
					trosakPresenter.azurirajTrosak(selectedTrosak);
					osvjeziInfo();
					opisView.Buffer.Text = t.Opis;
				};
				editWin.brisiTrosak += (sender2, e2) =>
				  {
					trosakPresenter.brisiTrosak(selectedTrosak);
				  };
			}
		}

	
			
		protected void backButtonClicked(object sender, EventArgs e)
		{
			datumChanged = null;

			notebook.CurrentPage = 0;
			if(backClicked != null)	backClicked(sender, e);
			backClicked = null;
			p = DateTime.Now.AddMonths(-1);
			k = DateTime.Now;

		}

		protected void datumFilterClicked(object sender, EventArgs e)
		{
			
			var	dCW = new DatumChooseWindow(p,k,this);
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
				nodeView.NodeStore = trosakPresenter;
				osvjeziInfo();
		}
		private void osvjeziInfo()
		{
			kategorijaLabel.LabelProp = currentKategorija;
			infoUkupno.LabelProp = trosakPresenter.Suma;
			infoProsjek.LabelProp = trosakPresenter.Prosjek;
		}
		public void setupTreeView()
		{
			
			var datumColumn = new TreeViewColumn("Datum", new CellRendererText(), "text", 0);

			var cijenaText = new CellRendererText();
			cijenaText.Xalign = 1;
			var cijenaColumn = new TreeViewColumn("Cijena", cijenaText, "text", 1);

			cijenaColumn.Clickable = true;
			cijenaColumn.Reorderable = true;
			cijenaColumn.SortIndicator = true;
			cijenaColumn.Resizable = true;
			cijenaColumn.MinWidth = 100;
			cijenaColumn.Alignment = 1;
			cijenaColumn.Clicked += (sender, e) => sortiranjePoCijeni(cijenaColumn);

			datumColumn.Clickable = true;
			datumColumn.Reorderable = true;
			datumColumn.SortIndicator = true;
			datumColumn.Resizable = true;
			datumColumn.SortOrder = SortType.Descending;
			datumColumn.MinWidth = 100;
			datumColumn.Clicked += (sender, e) => sortiranjePoDatumu(datumColumn);

			nodeView.AppendColumn(datumColumn);
			nodeView.AppendColumn(cijenaColumn);

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
			datumLabel.LabelProp = p.ToString("dd.MM.yyyy") + " - " + k.ToString("dd.MM.yyyy");
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
		private void addTotalTroskove()
		{

		}

		private void addStatisticView()
		{
			notebook.CurrentPage = 2;
			if (grafPresenter == null)
			{
				grafPresenter = new GrafPresenter();
				totalBar.Image = new Image("Bar", IconSize.Dnd);
				totalPie.Image = new Image("Pie", IconSize.Dnd);
				totalLine.Image = new Image("Line", IconSize.Dnd);
				kategorijaBar.Image = new Image("Bar", IconSize.Dnd);
				kategorijaLine.Image = new Image("Line", IconSize.Dnd);
				prikazPoBar.Image = new Image("Bar", IconSize.Dnd);
				prikazPoPie.Image = new Image("Pie", IconSize.Dnd);
				odabranaGodina.Text = DateTime.Now.Year.ToString();
				var kP = new KategorijaPresenter(kategorijeCombo);
			}
			datumChanged += (sender, e) => 
				datumLabela.LabelProp = p.ToString("dd.MM.yyyy") + " - " + k.ToString("dd.MM.yyyy");
			datumChanged(null, null);
		}

		protected void totalClicked(object sender, EventArgs e)
		{
			pocistiPlotBox();
			var b = sender as Button;

			if (b.Name == totalBar.Name)
			{
				plotBox.Add(grafPresenter.BarPlotSveKategorije(p, k));

			}
			else if (b.Name == totalPie.Name)
			{
				plotBox.Add(grafPresenter.PieViewSveKategorije(p, k));
			}

			else if (b.Name == totalLine.Name)
			{
				plotBox.Add(grafPresenter.PlotSveKategorije(p, k));
			}
			plotBox.ShowAll();
		}

		protected void kategorijaGrafClicked(object sender, EventArgs e)
		{
			pocistiPlotBox();
			var b = sender as Button;

			if (b.Name == kategorijaBar.Name)
			{
				plotBox.Add(grafPresenter.BarPlotKategorija(p, k, kategorijeCombo.ActiveText));
			}
			else if (b.Name == kategorijaLine.Name)
			{
				plotBox.Add(grafPresenter.PlotKategoriju(p, k,kategorijeCombo.ActiveText));
			}
			plotBox.ShowAll();
		}

		protected void prikazPoGrafClicked(object sender, EventArgs e)
		{
			pocistiPlotBox();
			var b = sender as Button;
			if (b.Name == prikazPoBar.Name)
			{
				plotBox.Add(grafPresenter.BarPlotMjeseceUGodini(Int32.Parse(odabranaGodina.Text)));
			}
			else if (b.Name == prikazPoPie.Name)
			{
				plotBox.Add(grafPresenter.PieViewMjeseceUGodini(Int32.Parse(odabranaGodina.Text)));
			}
			plotBox.ShowAll();
		}
		private void pocistiPlotBox()
		{
			var list = plotBox.AllChildren;
			foreach (Widget w in list)
			{
				plotBox.Remove(w);
			}
		}

		protected void openInNewWindowClicked(object sender, EventArgs e)
		{
			if (plotBox.Children.Length > 0)
			{
				var newWin = new Window("Osobni Troškovnik");
				var vbox = new VBox();
				var child = plotBox.Children[0];

				plotBox.Remove(child);

				newWin.Add(child);
				newWin.SetSizeRequest(600, 400);
				newWin.Icon = this.Icon;
				newWin.ShowAll();
			}

		}

		protected void brisiSveClicked(object sender, EventArgs e)
		{

			if (currentKategorija != null)
			{
				Dialog d = new Gtk.MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.None, "Time ćete obrisati sve postojeće troškove. Jeste li sigurni da želite obrisati sve troškove u kategoriji " + currentKategorija + "?");
				d.AddButton("Da", Gtk.ResponseType.Yes);
				d.AddButton("Ne", Gtk.ResponseType.No);

				var odgovor = (Gtk.ResponseType)d.Run();


				if (odgovor == Gtk.ResponseType.Yes)
				{
					d.Destroy();
					trosakPresenter.brisiSveTroskove(currentKategorija);
					MessageBox.Popout("Izbrisano", 2, this);

				}
				else d.Destroy();
			}
		}
	}
}
