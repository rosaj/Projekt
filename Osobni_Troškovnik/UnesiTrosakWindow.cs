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
			string ime = listaKategorija.ActiveText;
			Baza.getInstance.insertKategorija(ime);
			listaKategorija.AppendText(ime);
		}

		protected void novaKategorijaClicked(object sender, EventArgs e)
		{
			
		}
	}
}
