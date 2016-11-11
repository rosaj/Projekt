using System;
namespace Osobni_Troškovnik
{
	public class Trosak
	{
		private string kategorija;
		private float cijena;
		private string datum;
		private string opis;


		public Trosak(string kategorija, float cijena, string datum, string opis)
		{
			this.kategorija = kategorija;
			this.cijena = cijena;
			this.datum = datum;
			this.opis = opis;
		}



		public string Kategorija
		{
			get
			{
				return kategorija;
			}

		}

		public float Cijena
		{
			get
			{
				return cijena;
			}

		}

		public string Datum
		{
			get
			{
				return datum;
			}

		}

		public string Opis
		{
			get
			{
				return opis;
			}

		}
	}
}
