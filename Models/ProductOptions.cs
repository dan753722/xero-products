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
            LoadProductOptions(productId);
        }

        private void LoadProductOptions(Guid? productId)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            if (productId == null) {
                cmd.CommandText = "select id from productoptions";
            }
            else {
                cmd.CommandText = "select id from productoptions where productid = $productId collate nocase";
                cmd.Parameters.AddWithValue("$productId", productId);
            }
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new ProductOption(id));
            }
        }
    }
}