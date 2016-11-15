using System;
using System.Data.SQLite;
namespace Osobni_Troškovnik
{
	public abstract class DatabaseCreator
	{
		public static void createDatabase(string path, string connectionString)
		{


			try
			{
				SQLiteConnection.CreateFile(path);
				var con = new SQLiteConnection(connectionString);
				con.Open();

				string sql = "create table kategorija(id INTEGER PRIMARY KEY NOT NULL, ime varchar2(60) NOT NULL)";
				SQLiteCommand command = new SQLiteCommand(sql, con);
				command.ExecuteNonQuery();

				sql = "create table trosak(id INTEGER PRIMARY KEY NOT NULL," +
					"id_kategorija INTEGER NOT NULL," +
					"cijena NUMERIC NOT NULL," +
					"datum DATE NOT NULL," +
					"opis TEXT NOT NULL," +
					"FOREIGN KEY(id_kategorija) REFERENCES kategorija(id));";
				command = new SQLiteCommand(sql, con);
				command.ExecuteNonQuery();

				foreach (string s in Props.defultLista)
				{
					sql = String.Format("insert into kategorija(ime) values('{0}')", s);
					command = new SQLiteCommand(sql, con);
					command.ExecuteNonQuery();

				}

			}
			catch (SQLiteException e)
			{
				MessageBox.Show("Došlo je do greške spajanja na bazu " + e);
			}

		}

	}

}

/*
 *	private void createDatabase()
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
			MessageBox.Show("Došlo je do greške spajanja na bazu");
		}

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




*/
