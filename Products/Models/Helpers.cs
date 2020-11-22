using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Products.Models
{
    public class Helpers
    {
        private static SqliteConnection Connection;
        private const string ConnectionString = "Data Source=App_Data/products.db";

        public static SqliteConnection NewConnection()
        {
            if (Connection == null) {
                Connection = new SqliteConnection(ConnectionString);
            }

            return Connection;
        }
    }
}