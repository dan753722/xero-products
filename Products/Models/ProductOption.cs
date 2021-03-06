using System;
using Newtonsoft.Json;

namespace Products.Models
{
  public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select * from productoptions where id = $id collate nocase";
            cmd.Parameters.AddWithValue("$id", id);

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = IsNew
                ? $"insert into productoptions (id, productid, name, description) values ($id, $productId, $name, $description)"
                : $"update productoptions set name = $name, description = $description where id = $id collate nocase";

            cmd.Parameters.AddWithValue("$id", Id);
            cmd.Parameters.AddWithValue("$productId", ProductId);
            cmd.Parameters.AddWithValue("$name", Name);
            cmd.Parameters.AddWithValue("$description", Description);
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "delete from productoptions where id = $id collate nocase";
            cmd.Parameters.AddWithValue("$id", Id);
            cmd.ExecuteReader();
        }
    }
}