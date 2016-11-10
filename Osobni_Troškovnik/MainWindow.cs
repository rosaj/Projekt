using System;
using Gtk;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public partial class MainWindow : Gtk.Window
	{
		public	UnesiTrosakWindow uT;
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
			
			var lista = Baza.getInstance.getTroskove(ime);
			foreach (Trosak tr in lista)
			{
				Console.WriteLine(tr.Opis + " " + tr.Datum + " " + tr.Cijena);
			}

			var sW = new ScrolledWindow();
			var t = new Table((uint) lista.Count+3, 3, true);
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
			var sep = new HSeparator();
			t.Attach(sep, 1, 2, 1, 2, AttachOptions.Expand, AttachOptions.Fill, 0, 0);

			var lab = new Label("Opis");
			t.Attach(lab, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab.SetAlignment(0.1f, 0.5f);
			var lab1 = new Label("Datum");
			t.Attach(lab1, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab1.SetAlignment(0.7f, 0.5f);
			var lab2 = new Label("Cijena");
			t.Attach(lab2, 2, 3, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			lab2.SetAlignment(0.9f, 0.5f);



			Gdk.Color picked;
			for (int i = 3; i < lista.Count+3; i++)
			{
				var trosak = lista[i-3];

				var l = new Label(trosak.Opis);
				var l1 = new Label( trosak.Datum.ToShortDateString());
				var l2 = new Label(trosak.Cijena.ToString());
				l.SetAlignment(0, 0.5f);
				l1.SetAlignment(0.7f, 0.5f);
				l2.SetAlignment(0.9f, 0.5f);

				picked=(i % 2 == 0 ? Props.getColor("white") : Props.getColor("green")) ;


				var e = Props.add2EventBox(l,picked);
				var e1 = Props.add2EventBox(l1, picked);
				var e2 = Props.add2EventBox(l2, picked);


				t.Attach(e, 0, 1, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e1, 1, 2, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
				t.Attach(e2, 2, 3, (uint)i, (uint)(i + 1), AttachOptions.Fill, AttachOptions.Fill, 0, 0);
			}
			t.BorderWidth = 20;
			notebook.ShowAll();
			notebook.CurrentPage = 3;
		}






	}
}
