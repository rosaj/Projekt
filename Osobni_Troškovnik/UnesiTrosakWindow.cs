using System;
using System.Collections.Generic;
using Gtk;
namespace Osobni_Troškovnik
{



	public partial class UnesiTrosakWindow : Gtk.Window
	{
		public delegate void eventHandler();
		public event eventHandler signaliziraj;
		
		public UnesiTrosakWindow() :base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			this.Resizable = false;
			List<String> lista = Baza.getInstance.getKategorije();
			foreach (string s in lista) listaKategorija.AppendText(s);
			listaKategorija.Active = 0;
			cijena.Text = "";
	
		}


		protected void odustaniClicked(object sender, EventArgs e)
		{
			this.Destroy();
			signaliziraj();
		}

		protected void spremiAndNoviClicked(object sender, EventArgs e)
		{
			if (spremi())
			{
				cijena.Text = "";
				opis.Buffer.Text = "";
			}
		}

		protected void spremiClicked(object sender, EventArgs e)
		{
			if (spremi()) Destroy();
			signaliziraj();


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
		private bool spremi()
		{
			float broj;
			if (!float.TryParse(cijena.Text, out broj))
			{
				MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Cijena nesmije biti prazna");
				return false;
			}
			else if (broj.CompareTo(0) == 0)
			{
				MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Cijena nesmije biti nula");
				return false;
			}
			else if (opis.Buffer.Text.Length > 100)
			{
				MessageBox.Show(this, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Ok, "Opis nesmije biti duži od \n 100 znakova");
				return false;
			}
			else {
				
				Baza.getInstance.insertTrosak(listaKategorija.ActiveText, broj, kalendar.GetDate(),StringManipulator.insertBreaks(opis.Buffer.Text, 40));
				return true;
			}
		}


		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			signaliziraj();

		}

	}
}
