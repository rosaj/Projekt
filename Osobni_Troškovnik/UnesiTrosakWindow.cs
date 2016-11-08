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
		}


		protected void odustaniClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void spremiAndNoviClicked(object sender, EventArgs e)
		{
		}

		protected void spremiClicked(object sender, EventArgs e)
		{
			Baza.getInstance.insertTrosak(listaKategorija.ActiveText,Int32.Parse(cijena.Text), kalendar.GetDate(), opis.Buffer.Text);
		}

		protected void novaKategorijaClicked(object sender, EventArgs e)
		{
			var nova = new NovaKategorijaWidow();
			nova.Title = "Nova kategorija";
			nova.resurs += dodajKategoriju;
		}
		public void dodajKategoriju(string e)
		{
			Baza.getInstance.insertKategorija(e);
			listaKategorija.AppendText(e);
		}
	}
}
