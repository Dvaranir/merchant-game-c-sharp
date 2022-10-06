using MerchantGame.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Models
{
    internal class GoodsModel : Model
    {
        public static List<Good> GetAllGoods()
        {
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
