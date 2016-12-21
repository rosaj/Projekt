using System.Collections.Generic;
using Gtk;
using System;
namespace Osobni_Troškovnik
{
	public class KategorijaPresenter 

	{
		private static List< ComboBox> combo= new List<ComboBox>();

		public KategorijaPresenter(ComboBox cb)
		{
			
			foreach (var s in Kategorija.kategorije) cb.AppendText(s.Naziv);
			cb.Active = 0;
			combo.Add(cb);
		}
		public void insertKategorija( string kategorija)
		{
			bool match = false;
			kategorija = kategorija.Trim();
			foreach (var kat in Kategorija.kategorije)
			{
				if (kat.Naziv == kategorija)
				{
					match = true;
					break;
				}
			}
			if (!match)
			{
				var nova = new Kategorija(0, kategorija);
				Baza.getInstance.insertKategorija(nova.Naziv);
				nova.Id = Baza.getInstance.getKategorija(nova.Naziv).Id;
				Kategorija.kategorije.Add(nova);
				combo.ForEach((obj) => (obj as ComboBox).AppendText(nova.Naziv));
			}
			else 
			{
				throw new ArgumentException("Kategorija već postoji!");
			}
		}
		public static Kategorija getKategorija(string kategorija)
		{
			foreach (var kat in Kategorija.kategorije)
			{
				if (kat.Naziv == kategorija) return kat;
			}
			return null;
		}
	
	}

}
