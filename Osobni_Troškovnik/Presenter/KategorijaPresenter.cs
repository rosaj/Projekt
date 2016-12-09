
using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public class KategorijaPresenter 

	{
		ComboBox combo;
		public KategorijaPresenter(ComboBox cb)
		{
			var listaKat = Baza.getInstance.getKategorije();
			foreach (string s in listaKat) cb.AppendText(s);
			cb.Active = 0;
			combo = cb;
		}
		public bool insertKategorija( string kategorija)
		{
			if (Baza.getInstance.insertKategorija(kategorija))
			{
				combo.AppendText(kategorija);
				return true;
			}
			return false;
		}
	
	}

}
