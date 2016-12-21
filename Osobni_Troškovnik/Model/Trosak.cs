using System;
namespace Osobni_Troškovnik
{
	public class Trosak
	{
		private int id;
		private string kategorija;
		private double cijena;
		private DateTime datum;
		private string opis;


		public Trosak(int id, string kategorija, double cijena, DateTime datum, string opis)
		{
			ID = id;
			Kategorija = kategorija;
			Cijena = cijena;
			Datum = datum;
			Opis = opis;
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
				if (value.CompareTo(0) == 0)
				{
					throw new ArgumentException("Cijena mora biti veća od nule");
				}
				cijena = value;
			}

		}

		public DateTime Datum
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
				if (value.Length < 1)
				{
					throw new ArgumentException("Opis nesmije biti prazan");
				}
				opis = value;
			}
		}
	}
}
