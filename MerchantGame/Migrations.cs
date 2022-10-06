using MerchantGame.Entities;
using MerchantGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame
{
    internal class Migrations
    {
        public List<string> Tables { get; set; }
        public List<Good> Goods { get; set; }
        public List<City> Cities { get; set; }

        const string TableMerchant =
            "CREATE TABLE IF NOT EXISTS merchant (name VARCHAR(20) PRIMARY KEY, money INTEGER);";
        const string TableGoods =
            "CREATE TABLE IF NOT EXISTS goods (name VARCHAR(20) PRIMARY KEY, quality REAL, weight INTEGER, normal_quality_price INTEGER);";
        const string TableCities =
            "CREATE TABLE IF NOT EXISTS cities (name VARCHAR(20) PRIMARY KEY, distance INTEGER);";
        const string TableGoodsInCart =
            "CREATE TABLE IF NOT EXISTS goods_in_cart (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(20), quality REAL, weight INTEGER, normal_quality_price INTEGER);";

        readonly string[] GoodsNames = new string[] { "Meat", "Fruits", "Paint", "Flour", "Seeds", "Cloth" };
        readonly string[] CitiesNames = new string[] { "New York", "Almaty", "Toronto", "Berlin", "Paris", "London", "Sydney" };

        public Migrations() {
            Tables = new();
            Goods = new();
            Cities = new();
        }

        private void AddTables()
        {
            Tables.Add(TableMerchant);
            Tables.Add(TableGoods);
            Tables.Add(TableCities);
            Tables.Add(TableGoodsInCart);
        }

        private void CreateTables() {

            foreach (string table in Tables)
            {
                Model.UpdateDatabase(table);
            }
        }

        private bool InsertInDatabase<T>(List<T> Data, string TargetTable)
        {
            if (Data.Count == 0) return false;

            string InsertString = $"INSERT OR REPLACE INTO {TargetTable} VALUES ";

            StringBuilder stringBuilder = new(InsertString);

            foreach (T data in Data)
            {
                stringBuilder.Append('(');
                foreach (var property in data.GetType().GetProperties()) { stringBuilder.Append($"'{property.GetValue(data, null)}', ");
                    Console.WriteLine(property.GetValue(data));
                }
                stringBuilder.Length -= 2;
                stringBuilder.Append("), ");
            }

            stringBuilder.Length -= 2;
            stringBuilder.Append(';');

            string SqlRequest = stringBuilder.ToString();

            Model.UpdateDatabase(SqlRequest);

            return true;
        }

        private void GenerateGoods() =>
               Array.ForEach(GoodsNames, name => Goods.Add(new Good(name)));

        private void GenerateCities() =>        
            Array.ForEach(CitiesNames, name => Cities.Add(new City(name)));

        private void InsertGoodsInDatabase() =>
            InsertInDatabase(Goods, "goods");
        private void InsertCitiesInDatabase() =>
            InsertInDatabase(Cities, "cities");

        public void Migrate() {
            AddTables();
            CreateTables();
            GenerateGoods();
            GenerateCities();
            InsertGoodsInDatabase();
            InsertCitiesInDatabase();
        }
    }
}
