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
        public static List<City> GetAllCities()
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

                        OutputList.Add(new City(Name, Distance));
                    }
                }
                Connection.Close();
            }
            return OutputList;
        }

    }
}
