using System;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{



	public partial class UnesiTrosakWindow : Gtk.Window
	{
		
		public UnesiTrosakWindow() :base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			this.Resizable = false;
			var b =	Baza.getInstance;
			List<String> lista = Baza.getInstance.getKategorije();
			foreach (string s in lista) listaKategorija.AppendText(s);
			listaKategorija.Active = 0;
			cijena.Text = "";
		}


		protected void odustaniClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void spremiAndNoviClicked(object sender, EventArgs e)
		{
			spremi();
			cijena.Text = "";
			opis.Buffer.Text = "";

		}

		protected void spremiClicked(object sender, EventArgs e)
		{
			spremi();
			Baza.getInstance.ispis();
		}

		protected void novaKategorijaClicked(object sender, EventArgs e)
		{
			var nova = new NovaKategorijaWidow();
			nova.Title = "Nova kategorija";
			nova.resurs += dodajKategoriju;
		}
		public void dodajKategoriju(string e)
		{
			e = e.Trim();

			if(Baza.getInstance.insertKategorija(e))
							listaKategorija.AppendText(e);
		}
		private void spremi()
		{
			float broj;
			if (!float.TryParse(cijena.Text, out broj))
			{	 
					MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Cijena nesmije biti prazna");
			}
			else if (broj.CompareTo(0) == 0)
					MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Cijena nesmije biti nula");

			else 
					Baza.getInstance.insertTrosak(listaKategorija.ActiveText, broj, kalendar.GetDate(), opis.Buffer.Text);
		}
	}
}
