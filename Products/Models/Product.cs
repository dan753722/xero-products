using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Products.Models {
  public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Products where id = $id collate nocase";
            cmd.Parameters.AddWithValue("$id", id);

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                return;
            }

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            Price = decimal.Parse(rdr["Price"].ToString());
            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = IsNew
                ? "insert into Products (id, name, description, price, deliveryprice) values ($id, $name, $description, $price, $deliveryPrice)"
                : "update Products set name = $name, description = $description, price = $price, deliveryprice = $deliveryPrice where id = $id collate nocase";
            
            cmd.Parameters.AddWithValue("$id", Id);
            cmd.Parameters.AddWithValue("$name", Name);
            cmd.Parameters.AddWithValue("$description", Description);
            cmd.Parameters.AddWithValue("$price", Price);
            cmd.Parameters.AddWithValue("$deliveryPrice", DeliveryPrice);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = "delete from Products where id = $id collate nocase";
            cmd.Parameters.AddWithValue("$id", Id);
            cmd.ExecuteNonQuery();
        }
    }

}