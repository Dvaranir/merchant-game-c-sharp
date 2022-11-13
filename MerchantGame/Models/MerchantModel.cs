using MerchantGame.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Models
{
    internal class MerchantModel : Model
    {
        public static Merchant Get()
        {
            Merchant Player = new();
            string Request = "SELECT * FROM merchant";

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
                        int CartCapacity = Reader.GetInt32(1);
                        int CarryingWeight = Reader.GetInt32(2);
                        double Money = Reader.GetDouble(3);
                        double StartingMoney = Reader.GetDouble(4);
                        int DistanceLeft = Reader.GetInt32(5);
                        int DistanceTraveled = Reader.GetInt32(6);
                        string StartingCityName = Reader.GetString(7);
                        string DestinationCityName = Reader.GetString(8);
                        int DaysOnRoad = Reader.GetInt32(9);
                        byte GossipsEventAppeared = Reader.GetByte(10);

                        List <Good> GoodsIncart = GoodsInCartModel.Get(Name);
                        City DestinationCity = CitiesModel.Get(DestinationCityName);

                        Player.Init(
                            Name, CartCapacity, CarryingWeight, Money, 
                            StartingMoney, GoodsIncart, DistanceLeft, 
                            DistanceTraveled, StartingCityName, DestinationCity, 
                            DaysOnRoad, GossipsEventAppeared
                            );
                    }
                }
                Connection.Close();
            }
            return Player;
        }
        public static void Add(Merchant player)
        {
            string DestinationCityName = player.DestinationCity.Name;

            string Request = $"INSERT OR REPLACE INTO merchant VALUES ('{player.Name}', '{player.CartCapacity}', '{player.CarryingWeight}', '{player.Money}', '{player.StartingMoney}', '{player.DistanceLeft}', '{player.DistanceTraveled}', '{player.StartingCityName}', '{DestinationCityName}', '{player.DaysOnRoad}', '{player.GossipsEventAppeared}')";

            ExecuteRequest(Request);
        }
        
        public static void Drop()
        {
            string Request = "DELETE FROM merchant;";

            ExecuteRequest(Request);
        }

        public static void Drop(string name)
        {
            string Request = $"DELETE FROM merchant WHERE name = {name};";

            ExecuteRequest(Request);
        }
        public static bool Check()
        {
            string Request = $"SELECT 1 FROM merchant;";
            bool PlayerSaveExist = false;
            using (SqliteConnection Connection = new(ConnectionString))
            {
                Connection.Open();

                using (SqliteCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = Request;

                    SqliteDataReader Reader = Command.ExecuteReader();
                    PlayerSaveExist = Reader.HasRows;
                }
                Connection.Close();
            }
            return PlayerSaveExist;
        }
    }
}
