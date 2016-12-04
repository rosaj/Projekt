using System;
namespace Osobni_Troškovnik
{
	public class Trosak
	{
		private int id;
		private string kategorija;
		private double cijena;
		private string datum;
		private string opis;


		public Trosak(int id, string kategorija, double cijena, string datum, string opis)
		{
			this.id = id;
			this.kategorija = kategorija;
			this.cijena = cijena;
			this.datum = datum;
			this.opis = opis;
		}


		public int ID
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}
		public string Kategorija
		{
			get
			{
				return kategorija;
			}
			set
			{
				kategorija = value;
			}

		}

		public double Cijena
		{
			get
			{
				return cijena;
			}
			set
			{
				cijena = value;
			}

		}

		public string Datum
		{
			get
			{
				return datum;
			}
			set
			{
				datum = value;
			}

		}

		public string Opis
		{
			get
			{
				return opis;
			}

			set
			{
				opis = value;
			}
		}
	}
}
