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
			connectionString = string.Format("Data Source={0};version=3;datetimeformat=CurrentCulture",path);	
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
				createDatabase();
			}
			else {
				con = new SQLiteConnection(connectionString);
				con.Open();
				Console.WriteLine("spojeno");
			}
		}

		public bool insertKategorija(string s)
		{
			string sql = String.Format("select COUNT(*) from kategorija where LOWER(ime) LIKE LOWER('{0}')",s);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int br = Int32.Parse( reader[0].ToString());
			if (br == 0)
			{
				executeNonQuery(String.Format("insert into kategorija(ime) values('{0}')", s));
				return true;
			}
			return false;
		}

		public void insertTrosak(string kategorija, float cijena, DateTime datum, string opis)
		{
			string sql =string.Format( "select id from kategorija where LOWER(ime) LIKE LOWER('{0}')",kategorija);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int id = Int32.Parse(reader[0].ToString());
			executeNonQuery(string.Format(" insert into trosak(id_kategorija, cijena,datum,opis) " +
			                              "values('{0}','{1}','{2}','{3}')",id,cijena,datum,opis));
		
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



		public List<Trosak> getTroskove(string kategorija)
		{
			var lista = new List<Trosak>();


			string sql = string.Format("select id from kategorija where LOWER(ime) LIKE LOWER('{0}')", kategorija);
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			reader.Read();
			int id = Int32.Parse(reader[0].ToString());

			sql = string.Format("select cijena, datum, opis from trosak " +
			                    "where id_kategorija= '{0}'" +
			                    "order by datum asc", id);
			command = new SQLiteCommand(sql, con);
			reader = command.ExecuteReader();


			while (reader.Read()) 
			{
				
				var t = new Trosak(kategorija,float.Parse(reader[0].ToString()),
				                   DateTime.Parse( reader[1].ToString()) , 
				                   reader[2].ToString());
				lista.Add(t);
			}


			return lista;
		}

		private void createDatabase()
		{
			SQLiteConnection.CreateFile(path);
			try
			{
				con = new SQLiteConnection(connectionString);
				con.Open();

				string sql = "create table kategorija(id INTEGER PRIMARY KEY NOT NULL, ime varchar2(60) NOT NULL)";
				executeNonQuery(sql);
				sql = "create table trosak(id INTEGER PRIMARY KEY NOT NULL," +
					"id_kategorija INTEGER NOT NULL," +
					"cijena NUMERIC NOT NULL," +
					"datum DATE NOT NULL," +
					"opis TEXT NOT NULL," +
					"FOREIGN KEY(id_kategorija) REFERENCES kategorija(id));";
				executeNonQuery(sql);
				Console.WriteLine("Tablice kreirane");

				foreach (string s in Props.defultLista)
				{
					insertKategorija(s);
				}
				Console.WriteLine("insert kreirane");
			}
			catch (SQLiteException e)
			{
				Console.WriteLine(e.ToString());
			}

		}
		public void executeNonQuery(string sql)
		{
			SQLiteCommand command = new SQLiteCommand(sql, con);
			command.ExecuteNonQuery();
		}

		public void ispis()
		{
			string sql = "select * from trosak";
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();

			Console.WriteLine("");
			while (reader.Read())
			{

				DateTime d = (DateTime)reader[3];
				string s = d.ToString("dd/MM/yyyy");

				sql = string.Format("select ime from kategorija where id='{0}'", Int32.Parse(reader[1].ToString()));
				command = new SQLiteCommand(sql, con);
				SQLiteDataReader readerK = command.ExecuteReader();
				readerK.Read();

				Console.WriteLine("Id: {0} Kategorija: {1} Cijena: {2} Datum: {3} Opis: {4}", reader[0],readerK[0],reader[2], s, reader[4]);

			}


		}
		public void ispisiKategorije()
		{
			string sql = "select ime from kategorija";
			SQLiteCommand command = new SQLiteCommand(sql, con);
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read()) Console.WriteLine(reader[0]);
		}

	}



}
