using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Products.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts(name);
        }

        private void LoadProducts(string name)
        {
            Items = new List<Product>();
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            if (name == null) {
                cmd.CommandText = "select id from Products";
            }
            else {
                cmd.CommandText = "select id from Products where lower(name) like %$name%";
                cmd.Parameters.AddWithValue("$name", name.ToLower());
            }
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new Product(id));
            }
        }
    }
}