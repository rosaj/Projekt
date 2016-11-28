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

		public bool insertKategorija(string s)
		{
			string sql = String.Format("select COUNT(*) from kategorija where LOWER(ime) LIKE LOWER('{0}')", s);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int br = Int32.Parse(reader[0].ToString());
			if (br == 0)
			{
				executeNonQuery(String.Format("insert into kategorija(ime) values('{0}')", s));
				return true;
			}
			return false;
		}

		public void insertTrosak(string kategorija, float cijena, DateTime datum, string opis)
		{
			string sql = string.Format("select id from kategorija where LOWER(ime) LIKE LOWER('{0}')", kategorija);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();

			int id = Int32.Parse(reader[0].ToString());
			var dateString = datum.ToString("s");

			executeNonQuery(string.Format(" insert into trosak(id_kategorija, cijena,datum,opis) " +
										  "values('{0}','{1}','{2}','{3}')", id, cijena, dateString, opis));

		}


		public List<String> getKategorije()
		{
			string sql = "select ime from kategorija";
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			var list = new List<string>();

			while (reader.Read()) list.Add(reader[0].ToString());
			return list;
		}



		private void executeNonQuery(string sql)
		{
			var command = new SQLiteCommand(sql, con);
			command.ExecuteNonQuery();
		}

		public List<Trosak> getTroskoveURazdoblju(DateTime odDatum, DateTime doDatum, string kategorija)
		{

			var lista = new List<Trosak>();


			string sql = string.Format("select id from kategorija where LOWER(ime) LIKE LOWER('{0}')", kategorija);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int id = Int32.Parse(reader[0].ToString());

			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");
			sql = string.Format("select cijena, datum, opis from trosak " +
								"where id_kategorija= '{0}'" +
								" AND datum >= '{1}' AND datum <= '{2}' " +
								"order by datum DESC", id, datumOdString, datumDoString);
			command = new SQLiteCommand(sql, con);
			reader = command.ExecuteReader();


			while (reader.Read())
			{
				var dateString = DateTime.Parse(reader[1].ToString()).ToString("dd-MM-yyyy");

				var t = new Trosak(kategorija, float.Parse(reader[0].ToString()),
								   dateString, reader[2].ToString());
				lista.Add(t);
			}


			return lista;


		}
		/*public float getSumuTroskovaURazdoblju(DateTime odDatum, DateTime doDatum, string kategorija)
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

		public List<Trosak> getGrupiraneTroskoveURazdoblju(DateTime odDatum, DateTime doDatum, string kategorija)
		{

			var lista = new List<Trosak>();


			string sql = string.Format("select id from kategorija where LOWER(ime) LIKE LOWER('{0}')", kategorija);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int id = Int32.Parse(reader[0].ToString());

			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");
			sql = string.Format("select SUM(cijena),datum from trosak " +
								"where id_kategorija= '{0}'" +
								" AND datum >= '{1}' AND datum <= '{2}' " +
								"group by datum", id, datumOdString, datumDoString);
			command = new SQLiteCommand(sql, con);
			reader = command.ExecuteReader();


			while (reader.Read())
			{
				var dateString = DateTime.Parse(reader[1].ToString()).ToString("dd-MM-yyyy");

				var t = new Trosak(kategorija, float.Parse(reader[0].ToString()),
								   dateString, "");
				lista.Add(t);
			}


			return lista;


		}




		public Dictionary<string, float> getSumiraneTroskoveURazdoblju(DateTime odDatum, DateTime doDatum)
		{

			var lista = new Dictionary<string, float>();

			var datumOdString = odDatum.ToString("s");
			var datumDoString = doDatum.ToString("s");
			string sql = string.Format("select kategorija.ime, SUM(cijena) " +
									   "from trosak " +
									   "join kategorija " +
									   "ON id_kategorija = kategorija.id " +
									   "where datum >= '{0}' AND datum <= '{1}' " +
									   "group by ime " +
			                           "order by  sum(cijena) desc ", datumOdString, datumDoString);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{

				lista.Add(reader[0].ToString(), float.Parse(reader[1].ToString()));
			}


			return lista;


		}




	}

}
