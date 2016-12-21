using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
namespace Osobni_Troškovnik
{
	public class Baza
	{
		private static Baza db = new Baza();
		private SQLiteConnection con;
		private readonly string imeBaze = "troskovnik.sqlite";
		private string path;
		private string connectionString;

		private Baza()
		{
			path = System.IO.Path.Combine(Environment.CurrentDirectory, imeBaze);
			connectionString = string.Format("Data Source={0};version=3;datetimeformat=CurrentCulture", path);
			connectToDb();

		}

		public static Baza getInstance
		{
			get
			{
				return db;
			}
		}

		private void connectToDb()
		{

			if (!File.Exists(path))
			{
				DatabaseCreator.createDatabase(path, connectionString);
			}

			con = new SQLiteConnection(connectionString);
			con.Open();
	
		}
		public void closeCon()
		{
			con.Close();
		}

		public void  insertKategorija(string s)
		{
			
				executeNonQuery(String.Format("insert into kategorija(ime) values('{0}')", s));

		}
		public double getBudget()
		{
			string sql = "select iznos from budget";
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			return double.Parse(reader[0].ToString()) ;
		}
		public void setBudget(double budget)
		{

			executeNonQuery(string.Format("update budget set iznos = '{0}'",budget));
		}

		public void insertTrosak(Kategorija kategorija,Trosak t)
		{
			var dateString = t.Datum.ToString("s");

			executeNonQuery(string.Format(" insert into trosak(id_kategorija, cijena,datum,opis) " +
			                              "values('{0}','{1}','{2}','{3}')", kategorija.Id, t.Cijena, dateString, t.Opis));

		}

		public void updateTrosak(Trosak t)
		{
			
			var dateString = t.Datum.ToString("s");

			executeNonQuery(string.Format("update trosak set cijena = '{0}', datum = '{1}', opis = '{2}' " +
										  "where id = '{3}' ", t.Cijena, dateString, t.Opis, t.ID));

		}
		public void brisiTrosak(Trosak t)
		{
			executeNonQuery(string.Format("delete from trosak where id = '{0}' ", t.ID));

		}
		public void brisiSveTroskoveUKategoriji(Kategorija kategorija)
		{
			executeNonQuery(string.Format("delete from trosak where id_kategorija = '{0}' ",kategorija.Id));

		}

		public List<Kategorija> getKategorije()
		{
			string sql = "select id, ime from kategorija";
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			var list = new List<Kategorija>();

			while (reader.Read()) list.Add( new Kategorija(Int32.Parse(reader[0].ToString()),reader[1].ToString()));
			return list;
		}
		public Kategorija getKategorija(string kategorija)
		{
			string sql = string.Format("select id, ime from kategorija where ime = '{0}' ",kategorija);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();

			reader.Read();
			return new Kategorija(Int32.Parse(reader[0].ToString()), reader[1].ToString());
		}



		private void executeNonQuery(string sql)
		{
			var command = new SQLiteCommand(sql, con);
			command.ExecuteNonQuery();
		}

		public List<Trosak> getTroskoveURazdoblju(DateTime odDatum, DateTime doDatum,Kategorija kategorija)
		{

			var lista = new List<Trosak>();

			odDatum = odDatum.AddDays(-1);
			doDatum = doDatum.AddDays(+1);
			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");

			string	sql = string.Format("select id, cijena, datum, opis from trosak " +
								"where id_kategorija= '{0}'" +
								" AND datum > '{1}' AND datum < '{2}' " +
			                    "order by datum DESC", kategorija.Id, datumOdString, datumDoString);
			var command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();


			while (reader.Read())
			{
				var date = DateTime.Parse(reader[2].ToString());

				var t = new Trosak( Int32.Parse(reader[0].ToString()), 
				                   kategorija.Naziv, double.Parse(reader[1].ToString()),
								   date, reader[3].ToString());
				lista.Add(t);
			}


			return lista;


		}
		/*public double getSumuTroskovaURazdoblju(DateTime odDatum, DateTime doDatum, string kategorija)
		{

			string sql = string.Format("select id from kategorija where LOWER(ime) LIKE LOWER('{0}')", kategorija);
			var command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int id = Int32.Parse(reader[0].ToString());

			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");
			sql = string.Format("select SUM(cijena) from trosak " +
								"where id_kategorija= '{0}'" +
								" AND datum >= '{1}' AND datum <= '{2}' " +
								"order by datum DESC", id, datumOdString, datumDoString);
			command = new SQLiteCommand(sql, con);
			reader = command.ExecuteReader();
			float suma = 0;
			reader.Read();
			float.TryParse(reader[0].ToString(), out suma);

			return suma;



		}*/

		public List<Trosak> getGrupiraneTroskoveURazdoblju(DateTime odDatum, DateTime doDatum,Kategorija kategorija)
		{

			var lista = new List<Trosak>();

			odDatum = odDatum.AddDays(-1);
			doDatum = doDatum.AddDays(+1);

			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");
			string sql = string.Format("select SUM(cijena),datum from trosak " +
								"where id_kategorija= '{0}'" +
								" AND datum > '{1}' AND datum < '{2}' " +
								"group by datum", kategorija.Id, datumOdString, datumDoString);
			var command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();


			while (reader.Read())
			{
				var date = DateTime.Parse(reader[1].ToString());

				var t = new Trosak(0,	kategorija.Naziv, double.Parse(reader[0].ToString()),
								   date, "opis");
				lista.Add(t);
			}


			return lista;


		}




		public Dictionary<string, double> getSumiraneTroskoveURazdoblju(DateTime odDatum, DateTime doDatum)
		{

			var lista = new Dictionary<string, double>();

			odDatum = odDatum.AddDays(-1);
			doDatum = doDatum.AddDays(+1);
			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");
			string sql = string.Format("select kategorija.ime, SUM(cijena) " +
									   "from trosak " +
									   "join kategorija " +
									   "ON id_kategorija = kategorija.id " +
									   "where datum > '{0}' AND datum < '{1}' " +
									   "group by ime " +
			                           "order by  sum(cijena) desc ", datumOdString, datumDoString);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{

				lista.Add(reader[0].ToString(), double.Parse(reader[1].ToString()));
			}


			return lista;


		}

		public Dictionary<string, double> getTroskovePoKategorijamaUGodini(int godina)
		{
			var lista = new Dictionary<string,double>();

			string sql = string.Format("select strftime('%m',datum) as Mjesec, SUM(cijena) as Cijena " +
									   "from trosak " +
			                           "where strftime('%Y',datum) > '{0}' AND strftime('%Y',datum) <'{1}' " +
									   "group by Mjesec",godina-1,godina+1);



			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{

				int mjesec = Int32.Parse(reader[0].ToString());
				string mjesecString = "NA";
				switch (mjesec)
				{
					case 1:
						mjesecString = "Siječanj";
						break;
					case 2:
						mjesecString = "Veljača";
						break;		
					case 3:
						mjesecString = "Ožujak";
						break;
					case 4:
						mjesecString = "Travanj";
						break;	
					case 5:
						mjesecString = "Svibanj";
						break;
					case 6:
						mjesecString = "Lipanj";
						break;
					case 7:
						mjesecString = "Srpanj";
						break;
					case 8:
						mjesecString = "Kolovoz";
						break;
					case 9:
						mjesecString = "Rujan";
						break;
					case 10:
						mjesecString = "Listopad";
						break;
					case 11:
						mjesecString = "Studeni";
						break;
					case 12:
						mjesecString = "Prosinac";
						break;
				}

				lista.Add(mjesecString, double.Parse(reader[1].ToString()));

			}




			return lista;
		}




	}

}
