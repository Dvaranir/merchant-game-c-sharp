using MerchantGame.Entities;
using MerchantGame.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MerchantGame
{
    internal class Migrations
    {
        public List<string> Tables { get; set; }
        public List<Good> Goods { get; set; }
        public List<City> Cities { get; set; }
        public Shop Shop { get; set; }

        const string TableMerchant =
            "CREATE TABLE IF NOT EXISTS merchant (name VARCHAR(50) PRIMARY KEY, money INTEGER);";
        const string TableGoods =
            "CREATE TABLE IF NOT EXISTS goods (name VARCHAR(50) PRIMARY KEY, quality REAL, quality_tags VARCHAR(40), weight INTEGER, normal_quality_price INTEGER);";
        const string TableCities =
            "CREATE TABLE IF NOT EXISTS cities (name VARCHAR(50) PRIMARY KEY, distance INTEGER, required_goods TEXT);";
        const string TableGoodsInCart =
            "CREATE TABLE IF NOT EXISTS goods_in_cart (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(20), quality REAL, weight INTEGER, normal_quality_price INTEGER);";

        readonly string[] CitiesNames = new string[] { "New York", "Almaty", "Toronto", "Berlin", "Paris", "London", "Sydney" };

        public Migrations() {
            Tables = new();
            Goods = new();
            Cities = new();
            GenerateGoods();
            Shop = new();
            GenerateCities();
        }
        public void CreateDatabaseIfNotExist()
        {
            string CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            string DatabaseFilePath = $"{CurrentDirectory}/merchant.db";
            bool DatabaseExist = File.Exists(DatabaseFilePath);

            if (!DatabaseExist) Migrate();
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
                Model.ExecuteRequest(table);
            }
        }

        private void GenerateGoods() =>
               Array.ForEach(Settings.GetGoodsNames(), name => Goods.Add(new Good(name)));

        private void GenerateCities() 
        {
            foreach (string name in CitiesNames)
            {
                List<string> RequiredGoods = Shop.GenerateRequiredGoods();
                Cities.Add(new City(name, RequiredGoods));
            }
            
        }
            

        private void InsertGoodsInDatabase() =>
            Model.InsertInDatabase(Goods, "goods");
        private void InsertCitiesInDatabase() =>
            Model.InsertInDatabase(Cities, "cities");

        public void Migrate() {
            AddTables();
            CreateTables();
            InsertGoodsInDatabase();
            InsertCitiesInDatabase();
        }
    }
}
