using System;
namespace Osobni_Troškovnik
{
	public class TrosakNode : Gtk.TreeNode
	{
		

		[Gtk.TreeNodeValue(Column = 0)]
		public string datum;

		[Gtk.TreeNodeValue(Column = 1)]
		public string cijena;

		[Gtk.TreeNodeValue(Column = 2)]
		public string opis;

		public Trosak trosak;
		public TrosakNode(Trosak t)
		{
			
			this.trosak = t;
			datum = t.Datum.ToString("dd.MM.yyyy");
			cijena = t.Cijena.ToString("0.00 kn");
			opis = t.Opis;
		}
		public void updateTrosak(double cijena, DateTime datum, string opis)
		{
			trosak.Cijena = cijena;
			trosak.Datum = datum;
			trosak.Opis = opis;

			this.cijena = trosak.Cijena.ToString("0.00 kn");
			this.datum = trosak.Datum.ToString("dd.MM.yyyy");
			this.opis = trosak.Opis;
			Baza.getInstance.updateTrosak(this.trosak);
		}

	}
}