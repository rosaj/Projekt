using System.Collections.Generic;
using Gtk;
using System;
namespace Osobni_Troškovnik
{
	public class TrosakNodeStore : Gtk.NodeStore
	{
		private double suma;
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
		public void dodajTroskoveURazdoblju(string kategorija, DateTime pocetak, DateTime kraj)
		{
			var listaTr = KategorijaPresenter.getKategorija(kategorija).getTroskoveURazdoblju(pocetak,kraj);
			dodaj(listaTr);
		}
		public void brisiTroskove()
		{
			suma = 0;
			broj = 0;
			this.Clear();
		}

		public string Prosjek
		{
			get
			{
				return (suma / broj).ToString("0.00 kn");;
			}

		}
		public string Suma
		{
			get
			{
				return suma.ToString("0.00 kn");
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
					if (x.Cijena > y.Cijena) return -1;
					return 0;
				});
			}
			else 
			{
			lista.Sort((x, y) =>
				{
					if (x.Cijena < y.Cijena) return -1;
					if (x.Cijena > y.Cijena) return 1;
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
					
					if (x.Datum < y.Datum) return 1;
					if (x.Datum > y.Datum) return -1;
					return 0;
				});
			}
			else
			{
				lista.Sort((x, y) =>
					{
					if (x.Datum < y.Datum) return -1;
					if (x.Datum > y.Datum) return 1;
						return 0;
					});

			}
			this.Clear();
			dodaj(lista);


		}
		public void dodajNoviTrosak(string kategorija, double cijena, DateTime datum, string opis)
		{
			var kat = KategorijaPresenter.getKategorija(kategorija);
			Baza.getInstance.insertTrosak(kat , new Trosak(0, kat, cijena, datum, opis));
			
		}
		public void azurirajTrosak(TrosakNode tn, double cijena, DateTime datum, string opis)
		{
			tn.trosak.Cijena = cijena;
			tn.trosak.Datum = datum;
			tn.trosak.Opis = opis;

			tn.cijena = tn.trosak.Cijena.ToString("0.00 kn");
			tn.datum = tn.trosak.Datum.ToString("dd.MM.yyyy");
			tn.opis = tn.trosak.Opis;

			Baza.getInstance.updateTrosak(tn.trosak);
			suma = 0;broj = 0;
			foreach (TrosakNode t in this)
			{
				suma += t.trosak.Cijena;
				broj++;
			}

		}

		public void brisiTrosak(TrosakNode tn)
		{
			this.RemoveNode(tn);
			Baza.getInstance.brisiTrosak(tn.trosak);
		
		}
		public void brisiSveTroskove(string kategorija)
		{
			this.brisiTroskove();
			KategorijaPresenter.getKategorija(kategorija).brisiSveTroskove();
		}

	}

}
