using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public class KategorijeButtonPresenter
	{
		
		public VBox Kategorije(Callback c)
		{
			var kategorijeBox = new VBox(false, 10);
			var lista = Kategorija.kategorije;
			foreach (var s in lista)
			{
				string icon = s.Naziv;

				var b = ImageButton.imageButton(icon, s.Naziv);

				b.HeightRequest = 50;
				b.WidthRequest = 200;

				kategorijeBox.PackStart(b, false, false, 0);
				c.Invoke(b);

			}
			return kategorijeBox;
		}
	}
}
