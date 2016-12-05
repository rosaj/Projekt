
using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public class KategorijaPresenter 

	{
		
		public KategorijaPresenter(ComboBox cb)
		{
			var listaKat = Baza.getInstance.getKategorije();
			foreach (string s in listaKat) cb.AppendText(s);
			cb.Active = 0;
		}
	
	}

}
