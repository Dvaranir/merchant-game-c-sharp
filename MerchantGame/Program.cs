using System.Configuration;
using System.Collections.Specialized;
using MerchantGame;
using MerchantGame.Entities;
using MerchantGame.Models;

//Random Randomizer = new Random();

/*Merchant player = new Merchant("Oleg");
player.GoodsInCart.Add(new Good(20, "Watermellon", 20));

Console.WriteLine(player);
Console.WriteLine(player.CartCapacity);
Console.WriteLine(player.Name);*/

/*Model model = new Model();

model.CreacteTable();*/

Migrations migrations = new Migrations();

migrations.Migrate();
List<Good> Goods = GoodsModel.GetAllGoods();
Shop shop = new(Goods);
shop.GenerateRequiredGoods();


