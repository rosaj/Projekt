using Gtk;
using System;
namespace Osobni_Troškovnik
{
	public class TrosakTreePresenter: Gtk.TreeStore
	{
		private double ukupanTrosak;
		private string kategorija;
		private double trosakKategorije;
		private double budget;
		public TrosakTreePresenter() : base(typeof(string),typeof(string),typeof(string),typeof(string),typeof(double))
		{
			budget = Baza.getInstance.getBudget();
		}
		public void dodaj(DateTime p, DateTime k)
		{
			this.Clear();
			ukupanTrosak = 0;
			kategorija = "";
			trosakKategorije = 0;
			 
			TreeIter iter = new TreeIter();

			var lista = Baza.getInstance.getSumiraneTroskoveURazdoblju(p, k);

			foreach (var item in lista)
			{
				iter = this.AppendValues(item.Key,null,null,null,item.Value);

				foreach (var t in KategorijaPresenter.getKategorija(item.Key).getTroskoveURazdoblju(p,k))
					//Baza.getInstance.getTroskoveURazdoblju(p, k, item.Key))
				{
					var i = this.AppendValues(iter, null, t.Datum.ToString("dd.MM.yyyy"),t.Cijena.ToString("0.00 kn"));
					this.AppendValues(i, null, null, null, t.Opis);
					ukupanTrosak += t.Cijena;
				}
			}

		}




		
		public string UkupanTrosak
		{
			get
			{
				return ukupanTrosak.ToString("0.00 kn");
			}
		}
		public double total
		{
			get
			{
				return ukupanTrosak;
			}

		}
	
		public bool kategorijaChanged(TreeIter iter, TreeModel model)
		{
			
				var test = model.GetValue(iter, 0);
				if (test != null)
				{
					kategorija = test as string;
					var tr = model.GetValue(iter, 4);
					if (tr != null) trosakKategorije = (double)tr;
					return true;
				}

			return false;
		}
		public string Kategorija
		{
			get
			{
				return kategorija;
			}
		}
		public string TrosakTrenutneKategorije
		{
			get
			{
				return trosakKategorije.ToString("0.00 kn");
			}
		}

		public double trosakTrenutneKategorije
		{
			get
			{
				return trosakKategorije;
			}
		}
		public double Budget
		{
			get
			{
				return budget;
			}
			set
			{
				budget = value;
				Baza.getInstance.setBudget(value);
			}
		}
		public string BudgetString
		{
			get
			{
				return budget.ToString("0.00 kn");
			}
		}


	}
}
