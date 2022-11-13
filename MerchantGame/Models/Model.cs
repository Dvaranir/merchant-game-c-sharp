using System.Text;
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

        public static bool InsertInDatabase<T>(List<T> Data, string TargetTable)
        {
            if (Data.Count == 0) return false;

            string InsertString = $"INSERT OR REPLACE INTO {TargetTable} VALUES ";

            StringBuilder stringBuilder = new(InsertString);

            foreach (T data in Data)
            {
                stringBuilder.Append('(');
                foreach (var property in data.GetType().GetProperties())
                {
                    var PropertyValue = property.GetValue(data, null);

                    if (PropertyValue is List<string> PropertyList)
                    {
                        string PropertyString = string.Join(";", PropertyList);
                        stringBuilder.Append($"'{PropertyString}', ");
                    }
                    else
                    {
                        stringBuilder.Append($"'{PropertyValue}', ");
                    }
                }
                stringBuilder.Length -= 2;
                stringBuilder.Append("), ");
            }

            stringBuilder.Length -= 2;
            stringBuilder.Append(';');

            string SqlRequest = stringBuilder.ToString();

            ExecuteRequest(SqlRequest);

            return true;
        }

        public static void ExecuteRequest(string sqlRequest)
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
