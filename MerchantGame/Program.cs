using System.Configuration;
using System.Collections.Specialized;
using MerchantGame;
using MerchantGame.Entities;
using MerchantGame.Models;

string CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory());
string DatabaseFilePath = $"{CurrentDirectory}/merchant.db";
bool DatabaseExist = File.Exists(DatabaseFilePath);

if (!DatabaseExist)
{
    Migrations Migrations = new();
    Migrations.Migrate();
}


Merchant Player = new();
List<Good> Goods = GoodsModel.GetAllGoods();
Shop shop = new(Goods);
shop.GenerateRequiredGoods();


