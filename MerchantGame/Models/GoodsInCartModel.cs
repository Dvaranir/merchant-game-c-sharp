using MerchantGame.Entities;
using Microsoft.Data.Sqlite;
using System.Text;

namespace MerchantGame.Models
{
    internal class GoodsInCartModel : Model
    {

        public static List<Good> Get(string name)
        {
            List<Good> OutputList = new();
            string Request = $"SELECT * FROM goods_in_cart WHERE player_name = '{name}'";

            using (SqliteConnection Connection = new(ConnectionString))
            {
                Connection.Open();

                using (SqliteCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = Request;

                    SqliteDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        string Id = Reader.GetString(0);
                        string Name = Reader.GetString(1);
                        float Quality = Reader.GetFloat(2);
                        string QualityTag = Reader.GetString(3);
                        byte Weight = Reader.GetByte(4);
                        int Price = Reader.GetInt32(5);

                        OutputList.Add(new Good(Name, Quality,QualityTag, Weight, Price, Id));
                    }
                }
                Connection.Close();
            }
            return OutputList;
        }
        public static void Add(List<Good> goodsInCart, string playerName)
        {
            StringBuilder stringBuilder = new("INSERT OR REPLACE INTO goods_in_cart VALUES ");
            foreach (Good good in goodsInCart)
            {
                stringBuilder.Append($"('{good.Id}', '{good.Name}', '{good.Quality}', '{good.QualityTag}', '{good.Weight}', '{good.Price}', '{playerName}'), ");
            }
            string Request = stringBuilder.ToString();
            Request = Request.Substring(0, Request.Length - 2) + ';';
            
            ExecuteRequest(Request);
        }

        
    }
}
