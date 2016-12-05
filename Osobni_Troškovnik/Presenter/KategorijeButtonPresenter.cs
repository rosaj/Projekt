using System;
using Gtk;
namespace Osobni_Troškovnik
{
	public class KategorijeButtonPresenter
	{
		public KategorijeButtonPresenter()
		{
		}
		public VBox Kategorije(Callback c)
		{
			var kategorijeBox = new VBox(false, 10);
			var lista = Baza.getInstance.getKategorije();
			foreach (var s in lista)
			{
				string icon = s;
				if (!DatabaseCreator.defultLista.Contains(s))
					icon = "r";
				var b = ImageButton.imageButton(icon, s);

				b.HeightRequest = 50;
				b.WidthRequest = 200;

				kategorijeBox.PackStart(b, false, false, 0);
				c.Invoke(b);

			}
			return kategorijeBox;
		}
	}
}
