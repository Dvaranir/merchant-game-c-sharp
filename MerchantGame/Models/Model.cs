﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MerchantGame.Entities;
using Microsoft.Data.Sqlite;

namespace MerchantGame
{
    internal class Model
    {
        static string CurrentDirrectory = Path.Combine(Directory.GetCurrentDirectory());
        protected static string ConnectionString = $@"Data Source={CurrentDirrectory}/merchant.db";
        public string SqlRequest { get; set; }


        public Model() {
            SqlRequest = "";
        }

        public static void UpdateDatabase(string sqlRequest)
        {
            using (SqliteConnection Connection = new SqliteConnection(ConnectionString))
            {
                Connection.Open();

                using (SqliteCommand Command = Connection.CreateCommand()) 
                { 

                Command.CommandText = sqlRequest;
                Command.ExecuteNonQuery();

                }
                Connection.Close();
            }
        }

        public static List<Good> GetAllGoods() {

            List<Good> OutputList = new List<Good>();
            string Request = "SELECT * FROM goods";

            using (SqliteConnection Connection = new SqliteConnection(ConnectionString))
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
                        byte Weight = Reader.GetByte(2);
                        int Price = Reader.GetInt32(3);

                        OutputList.Add(new Good(Name, Quality, Weight, Price));
                    }
                }
                Connection.Close();
            }
            return OutputList;
        }
        

    }
}