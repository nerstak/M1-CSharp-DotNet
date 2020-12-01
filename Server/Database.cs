using System;
using Npgsql;

namespace Server
{
    public class Database
    {
        private static string Host = "localhost";
        private static string User = "adm";
        private static string DBname = "csharp_db";
        private static string Password = "adm";
        private static string Port = "5432";

        private NpgsqlConnection conn;

        public void connect()
        {
            string connString = 
                String.Format(
                    "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            conn = new NpgsqlConnection(connString);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM USERS",conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(
                    string.Format(
                        "Reading from table=({0}, {1})",
                        reader.GetString(0),
                        reader.GetString(1)
                    )
                );
            }
        }
    }
}