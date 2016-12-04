using System.Collections.Generic;
using Gtk;
using System;
namespace Osobni_Troškovnik
{
	public class TrosakNodeStore : Gtk.NodeStore
	{
		public double suma { get; set; }
		private int broj;
		public TrosakNodeStore() : base(typeof(TrosakNode))
		{
			broj = 0;
			suma = 0;
		}
		public void Add(Trosak t)
		{
			this.AddNode(new TrosakNode(t));
			suma += t.Cijena;
			broj++;
		}
		public void dodaj(List<Trosak> lista)
		{
			foreach (var t in lista)
			{
				this.Add(t);
			}
		}
		public void brisiTroskove()
		{
			suma = 0;
			broj = 0;
			this.Clear();
		}

		public double Prosjek
		{
			get
			{
				return suma / broj;
			}

		}
		public void sortByCijena(SortType st) 
		{
			var lista = new List<Trosak>();
			foreach (TrosakNode tn in this)
			{
				lista.Add(tn.trosak);	
			}

			if (st == SortType.Descending)
			{
				lista.Sort((x, y) =>
				{
					if (x.Cijena < y.Cijena) return 1;
					else if (x.Cijena > y.Cijena) return -1;
					return 0;
				});
			}
			else 
			{
			lista.Sort((x, y) =>
				{
					if (x.Cijena < y.Cijena) return -1;
					else if (x.Cijena > y.Cijena) return 1;
					return 0;
				});

			}
			this.Clear();
			dodaj(lista);
		}
		public void sortByDatum(SortType st)
		{

			var lista = new List<Trosak>();
			foreach (TrosakNode tn in this)
			{
				lista.Add(tn.trosak);
			}

			if (st == SortType.Descending)
			{
				lista.Sort((x, y) =>
				{
					var datumX = DateTime.Parse(x.Datum);
					var datumY = DateTime.Parse(y.Datum);
					if (datumX < datumY) return 1;
					else if (datumX > datumY) return -1;
					return 0;
				});
			}
			else
			{
				lista.Sort((x, y) =>
					{
						var datumX = DateTime.Parse(x.Datum);
						var datumY = DateTime.Parse(y.Datum);
						if (datumX < datumY) return -1;
						else if (datumX > datumY) return 1;
						return 0;
					});

			}
			this.Clear();
			dodaj(lista);


		}

	}

}
