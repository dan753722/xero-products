using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Products.Models
{
  public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}' collate nocase");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = where == null ? "select id from productoptions" : "select id from productoptions $where";
            cmd.Parameters.AddWithValue("$where", where);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new ProductOption(id));
            }
        }
    }
}