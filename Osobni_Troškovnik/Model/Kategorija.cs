using System.Collections.Generic;
using System;
namespace Osobni_Troškovnik
{
	public class Kategorija
	{
		private int id;
		private string naziv;
		public static List<Kategorija> kategorije = Baza.getInstance.getKategorije();
		public Kategorija(int id, string naziv)
		{
			Id = id;
			Naziv = naziv;
		}

		public int Id
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

		public string Naziv
		{
			get
			{
				return naziv;
			}

			set
			{
				if (value.Length < 1 )
				{
					throw new ArgumentException("Naziv kategorije nesmije biti prazan");
				}
				naziv = value;
			}
		}
		public List<Trosak> getTroskoveURazdoblju(DateTime odDatum, DateTime doDatum)
		{
			return	Baza.getInstance.getTroskoveURazdoblju(odDatum, doDatum, this);
		}
		public void brisiSveTroskove()
		{
			Baza.getInstance.brisiSveTroskoveUKategoriji(this);
		}
	}
}
