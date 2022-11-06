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
        public List<City> Cities { get; set;}
        public Merchant Player { get; set; }
        public Shop Shop { get; set; }
        public Events Events { get; set; }
        public Migrations Migrations { get; set; }

        public Controller ()
        {
            Cities = CitiesModel.GetAllCities();
            Player = new(GetRandomCityName(), GetRandomCity());
            Shop = new();
            Events = new(Player, Shop);
            Migrations = new();
        }


        public void StartNewGame()
        {
            Migrations.CreateDatabaseIfNotExist();

            InitialPurchase();
            MainLoop();
            Console.WriteLine("Finished");
            
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
                Console.WriteLine($"{Player.Name} bought {RandomGood.Name}");
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

        public City GetRandomCity() =>
            Cities[Random.Shared.Next(0, Cities.Count - 1)];

        public string GetRandomCityName() =>
            Cities[Random.Shared.Next(0, Cities.Count - 1)].Name;
        
    }
}
