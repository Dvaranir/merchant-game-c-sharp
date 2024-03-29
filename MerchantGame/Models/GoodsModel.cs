﻿using MerchantGame.Entities;
using Microsoft.Data.Sqlite;

namespace MerchantGame.Models
{
    internal class GoodsModel : Model
    {
        public static List<Good> Get()
        {
            List<Good> OutputList = new();
            string Request = "SELECT * FROM goods";

            using (SqliteConnection Connection = new(ConnectionString))
            {
                Connection.Open();

                using (SqliteCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = Request;

                    SqliteDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        string Name = Reader.GetString(0);
                        float Quality = Reader.GetFloat(1);
                        byte Weight = Reader.GetByte(3);
                        int Price = Reader.GetInt32(4);

                        OutputList.Add(new Good(Name, Quality, Weight, Price));
                    }
                }
                Connection.Close();
            }
            return OutputList;
        }

        public static void Update(string name, byte weight, int price) 
        {
            string Request = $"UPDATE goods SET weight = '{weight}', normal_quality_price = '{price}' WHERE name = '{name}'";

            ExecuteRequest(Request);
        }
        public static void Add(string name, byte weight, int price)
        {
            Good Good = new(name, weight, price);
            string Request = $"INSERT INTO goods VALUES ('{Good.Name}', '{Good.Quality}', '{Good.QualityTag}', '{Good.Weight}', '{Good.Price}', '{Good.Id}')";
            
            ExecuteRequest(Request);
        }
    }
}
