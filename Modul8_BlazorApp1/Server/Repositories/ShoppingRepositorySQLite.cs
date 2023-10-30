﻿using System;
using System.Xml.Linq;
using Modul8_BlazorApp1.Shared;
using Microsoft.Data.Sqlite;
using Modul8_BlazorApp1.Client.Pages;
using Modul8_BlazorApp1.Server.Repositories;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace Modul8_BlazorApp1.Server.Repositories
{
    public class ShoppingRepositorySQLite : IShoppingRepository
    {
        private const string connectionString = @"Data Source=C:\Users\rasmu\OneDrive\Skrivebord\ShoppingItem.db";

        public ShoppingRepositorySQLite()
        {
        }
        public void AddItem(ShoppingItem item)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = @"INSERT INTO ShoppingItem (Name, Price, Amount, Shop, Description, Done) VALUES ($name, $price, $amount, $shop, $desc, $done)";
                command.Parameters.AddWithValue("$name", item.Name);
                command.Parameters.AddWithValue("$price", item.Price);
                command.Parameters.AddWithValue("$amount", item.Amount);
                command.Parameters.AddWithValue("$shop", item.Shop);
                command.Parameters.AddWithValue("$desc", item.Description);
                command.Parameters.AddWithValue("$done", item.Done);
                command.ExecuteNonQuery();
            }
        }
        public void DeleteById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = @"DELETE FROM ShoppingItem WHERE id = $id";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
            }
        }

        public ShoppingItem[] GetAll()
        {
            var result = new List<ShoppingItem>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM ShoppingItem";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Id = reader.GetInt32(0);
                        var Name = reader.GetString(1);
                        var Price = reader.GetInt32(2);
                        var Amount = reader.GetInt32(3);
                        var Shop = reader.GetString(4);
                        var Description = reader.GetString(5);
                        var Done = reader.GetInt32(6) == 0 ? false : true;


                        ShoppingItem b = new ShoppingItem { Id = Id, Name = Name, Price = Price, Amount = Amount, Shop = Shop, Description = Description, Done = Done };
                        result.Add(b);
                    }
                }

            }
            return result.ToArray();
        }
        public void UpdateItem(ShoppingItem item)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = @"UPDATE shoppingitem SET Done = $done WHERE id = $id";
                command.Parameters.AddWithValue("$id", item.Id);
                command.Parameters.AddWithValue("$done", item.Done ? 1 : 0);
                command.ExecuteNonQuery();
            }
        }
    }
}
