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

		public void insertKategorija(string s)
		{
			executeNonQuery(String.Format("insert into kategorija(ime) values('{0}')", s));

		}

		public void insertTrosak(string kategorija, float cijena, DateTime datum, string opis)
		{
			string sql =string.Format( "select id from kategorija where ime='{0}'",kategorija);
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

		private void createDatabase()
		{
			SQLiteConnection.CreateFile(path);
			try
			{
				con = new SQLiteConnection(connectionString);
				con.Open();

				string sql = "create table kategorija(id INTEGER PRIMARY KEY NOT NULL, ime varchar2(60))";
				executeNonQuery(sql);
				sql = "create table trosak(id INTEGER PRIMARY KEY NOT NULL," +
					"id_kategorija INTEGER NOT NULL," +
					"cijena NUMERIC," +
					"datum DATE," +
					"opis TEXT," +
					"FOREIGN KEY(id_kategorija) REFERENCES kategorija(id));";
				executeNonQuery(sql);
				Console.WriteLine("Tablice kreirane");

				var lista = new List<string>() { "Hrana", "Skolovanje", "Gorivo","Automobil", "Namjestaj", "Stanarina",
					"Računalna oprema", "Struja", "Voda", "Telefon", "Internet", "TV", "Odjeća", "Nakit", "Shopping", "Zdravlje"};
				foreach (string s in lista)
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
				

				Console.WriteLine("{0} {1} {2} {3} {4}", reader[0],reader[1],reader[2], reader[3], reader[4]);

			}


		}

	}



}
