using System.Collections.Generic;
using Gtk;
using System;
namespace Osobni_Troškovnik
{
	public static  class KategorijaPresenter 

	{
		private static List< ComboBox> combo= new List<ComboBox>();


		public static void generirajKategorije(ComboBox cb)
		{
			foreach (var s in Kategorija.kategorije) cb.AppendText(s.Naziv);
			cb.Active = 0;
			combo.Add(cb);
		}
		public static void otpustiComboBox(ComboBox cb)
		{
			combo.Remove(cb);
		}
		public static void insertKategorija( string kategorija)
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
				nova.Id =	Baza.getInstance.insertKategorija(nova.Naziv);
				Kategorija.kategorije.Add(nova);
				combo.ForEach((obj) => (obj as ComboBox).AppendText(nova.Naziv));
				combo.ForEach((obj) => { 
					Console.WriteLine((obj as ComboBox).Name);});
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
