using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MerchantGame.Entities;
using MerchantGame.Models;

namespace MerchantGame
{
    internal class Controller
    {
        public City[] Cities { get; set;}
        public Merchant Player { get; set; }
        public Shop Shop { get; set; }
        public Events Events { get; set; }
        public Migrations Migrations { get; set; }
        public City DestinationCity { get; set; }

        public Controller ()
        {
            Cities = CitiesModel.GetAllCities();
            DestinationCity = GetRandomCity();
            Player = new(GetRandomCityName(), DestinationCity);
            Shop = new();
            Events = new(Player, Shop, Cities.ToArray());
            Migrations = new();
        }


        public void StartGame()
        {
            Migrations.CreateDatabaseIfNotExist();

            InitialPurchase();
            MainLoop();
            SellAllGoods();
            EndGameStatistic();
        }

        public void ContinueGame()
        {
            List<Good> Goods = GoodsModel.GetAllGoods();
        }

        public void InitialPurchase()
        {
            int LowestPrice = Shop.GetGoodLowestPrice();
            int LowestWeight = Shop.GetGoodLowestWeight();

            while (true)
            {
                Good RandomGood = Events.ChooseGoodForPlayer();
                Player.BuyGood(Events.ChooseGoodForPlayer());
                Console.WriteLine($"{Player.Name} bought {RandomGood.Name} for {RandomGood.Price}$");
                int SpaceInCartLeft = Player.CartCapacity - Player.CarryingWeight;

                if (Player.Money < LowestPrice ||
                    SpaceInCartLeft < LowestWeight) break;
            }
        }

        public void MainLoop()
        {
            while (true)
            {
                Events.RandomEvent();
                
                if (Player.DistanceLeft <= 0)
                {
                    Player.DistanceLeft = 0;
                    break;
                }
            }
        }

        public void SellAllGoods()
        {
            byte FirstProductInCart = 0;
            while (true)
            {
                if (Player.GoodsInCart.Count == 0) break;

                string GoodName = Player.GoodsInCart[0].Name;
                if (DestinationCity.RequiredGoods.Contains(GoodName))
                    Player.SellGood(FirstProductInCart, Settings.CityRequiredGoodsPriceModifier);
                
                else Player.SellGood();
            }
        }

        public void EndGameStatistic()
        {
            double StartingMoney = Player.StartingMoney;
            double Money = Player.Money;

            Console.WriteLine($"{Player.Name} passed {Player.DistanceTraveled}km");
            Console.WriteLine($"{Player.Name} have {(int) Player.StartingMoney}$ at start");
            Console.WriteLine($"{Player.Name} have {(int) Player.Money}$ now");
            if (StartingMoney < Money)
                Console.WriteLine($"{Player.Name} earned {(int) (Player.Money - Player.StartingMoney)}$");
            else
                Console.WriteLine($"{Player.Name} earned nothing");
        }

        public City GetRandomCity() =>
            Cities[Random.Shared.Next(0, Cities.Length - 1)];

        public string GetRandomCityName() =>
            Cities[Random.Shared.Next(0, Cities.Length - 1)].Name;
        
    }
}
