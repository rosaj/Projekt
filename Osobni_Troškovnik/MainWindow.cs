using System;
using Gtk;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		private	UnesiTrosakWindow uT;
		private DatumChooseWindow dCW;
		public MainWindow() :base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			notebook.CurrentPage = 0;

		}
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

		protected void noviTrosakClicked(object sender, EventArgs e)
		{
			if (uT == null)
			{
				uT = new UnesiTrosakWindow();
				uT.signaliziraj+= () => uT=null;
			}
		}
		protected void statistikaClicked(object sender, EventArgs e)
		{
			notebook.CurrentPage = 1;
			generirajKategorije();
		}

		protected void totalCostClicked(object sender, EventArgs e)
		{
			var lista = Baza.getInstance.getTroskove("gorivo");
			foreach (Trosak s in lista) Console.WriteLine(s.Datum);
		}

		protected void izvjesceClicked(object sender, EventArgs e)
		{
		}

		protected void izlazClicked(object sender, EventArgs e)
		{
			Destroy();
		}
		private void generirajKategorije()
		{
			var lista = Baza.getInstance.getKategorije();
			var sW = new ScrolledWindow();
			var t = new Table((uint)(lista.Count+2), 1, true);
			sW.AddWithViewport(t);
			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;
			var back = new Button(ImageButton.imageButton("gtk-go-back"));
			t.Attach(back, 0, 1, 0, 1, AttachOptions.Expand, AttachOptions.Fill,0,0);
			back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 0;
			};
			var sep = new HSeparator();
			t.Attach(sep, 0, 1,1, 2, AttachOptions.Expand, AttachOptions.Fill, 0, 0);





			for (int i = 2; i <= lista.Count+1; i++)
			{
				string s = lista[i-2];
				string icon = s;
				if (!Props.defultLista.Contains(s))
													icon = "r";
				var b = ImageButton.imageButton(icon, s);


				b.HeightRequest = 50;
				b.WidthRequest = 400;
			
				t.Attach(b, 0, 1, (uint)i,(uint) (i + 1), AttachOptions.Expand, AttachOptions.Fill,0 ,0);
				b.Clicked += (sender, e) => kategorijaClicked(((Button)sender).Name);
			}

			notebook.ShowAll();
			notebook.CurrentPage = 2;
		}

		private void kategorijaClicked(string ime)
		{
			addTroskove(Baza.getInstance.getTroskove(ime), ime);

		}

		private void addTroskove(List<Trosak> lista, string ime)
		{


			var sW = new ScrolledWindow();
			var t = new Table((uint)lista.Count + 3, 3, true);
			sW.AddWithViewport(t);
			sW.SetPolicy(PolicyType.Never, PolicyType.Automatic);
			notebook.Add(sW);

			t.RowSpacing = 10;
			t.WidthRequest = 800;

			var back = new Button(ImageButton.imageButton("gtk-go-back"));
			t.Attach(back, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Fill, 0, 0);

			back.SetSizeRequest(400, 50);
			back.Clicked += (sender, e) =>
			{
				notebook.Remove(sW);
				notebook.CurrentPage = 2;
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
						addTroskove(Baza.getInstance.getTroskoveURazdoblju(odDatum, doDatum, ime), ime);
						dCW = null;
					};
				}
			
			};

			t.Attach(rangeButton, 1, 2, 1, 2, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);

			var lab = new Label("Opis");
			t.Attach(lab, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0f, 0.5f);
			var lab1 = new Label("Datum");
			t.Attach(lab1, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			//lab1.SetAlignment(0.7f, 0.5f);
			var lab2 = new Label("Cijena");
			t.Attach(lab2, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.9f, 0.5f);


			float cijena = 0;
			Gdk.Color picked;
			for (int i = 3; i < lista.Count + 3; i++)
			{
				var trosak = lista[i - 3];

				var l = new Label(trosak.Opis);
				var l1 = new Label(trosak.Datum);
				var l2 = new Label(trosak.Cijena.ToString());
				l.SetAlignment(0, 0.5f);
				//l1.SetAlignment(0.7f, 0.5f);
				l2.SetAlignment(0.9f, 0.5f);

				picked = (i % 2 == 0 ? Props.getColor("#E7FFB3") : Props.getColor("#C4FFE4"));



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




	}
}
