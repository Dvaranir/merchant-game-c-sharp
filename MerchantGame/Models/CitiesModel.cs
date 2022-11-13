using MerchantGame.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Models
{
    internal class CitiesModel : Model
    {
        public CitiesModel() {}
        public static City Get(string name)
        {
            string Request = $"SELECT * FROM cities WHERE name = '{name}'";
            City OutputCity = new();

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
                        byte Distance = Reader.GetByte(1);
                        List<string> RequredGoods = Reader.GetString(2).Split(';').ToList();

                        OutputCity.Name = Name;
                        OutputCity.Distance = Distance;
                        OutputCity.RequiredGoods = RequredGoods;
                    }
                }
                Connection.Close();
            }

            return OutputCity;
        }  
        public static City[] GetAll()
        {

            List<City> OutputList = new List<City>();
            string Request = "SELECT * FROM cities";

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
                        byte Distance = Reader.GetByte(1);
                        List<string> RequredGoods = Reader.GetString(2).Split(';').ToList();

                        OutputList.Add(new City(Name, Distance, RequredGoods));
                    }
                }
                Connection.Close();
            }
            return OutputList.ToArray();
        }
       

    }
}
