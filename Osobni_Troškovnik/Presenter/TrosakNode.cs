namespace Osobni_Troškovnik
{
	public class TrosakNode : Gtk.TreeNode
	{
		public Trosak trosak;

		[Gtk.TreeNodeValue(Column = 0)]
		public string datum;

		[Gtk.TreeNodeValue(Column = 1)]
		public string cijena;

		[Gtk.TreeNodeValue(Column = 2)]
		public string opis;


		public TrosakNode(Trosak t)
		{

			this.trosak = t;
			datum = t.Datum;
			cijena = t.Cijena.ToString("0.00 kn");
			opis = t.Opis;
		}

	}
}