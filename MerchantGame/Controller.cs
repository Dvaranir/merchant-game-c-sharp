using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            Migrations = new();
            Migrations.CreateDatabaseIfNotExist();

            Cities = CitiesModel.GetAllCities();
            DestinationCity = GetRandomCity();
            Player = new(GetRandomCityName(), DestinationCity);
            Shop = new();
            Events = new(Player, Shop, Cities.ToArray());
        }

        public void StartGame()
        {
            ChangePlayerStats();
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
            Cities[Random.Shared.Next(0, Cities.Length)];

        public string GetRandomCityName() =>
            Cities[Random.Shared.Next(0, Cities.Length)].Name;

        public void ChangePlayerStats()
        {
            Console.WriteLine("Write cart capacity:");
            int CartCapacity = Events.GetIntegerInputFromUser();

            Console.WriteLine("Write starting money:");
            int StartingMoney = Events.GetIntegerInputFromUser();


            Console.WriteLine("Choose starting city:");
            StringBuilder StringBuilder = new();

            for (int i = 0; i < Cities.Length; i++)
            {
                StringBuilder.Append(i + 1);
                StringBuilder.Append(") ");
                StringBuilder.Append(Cities[i].Name);
                StringBuilder.Append("\n");
            }
            Console.WriteLine(StringBuilder.ToString());

            int StartingCityChoice = Events.GetByteInputFromUser((byte) Cities.Length);

            Player.CartCapacity = CartCapacity;
            Player.StartingMoney = StartingMoney;
            Player.Money = StartingMoney;
            Player.StartingCityName = Cities[StartingCityChoice - 1].Name;
        }

        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("1) Continue");
            Console.WriteLine("2) New Game");
            Console.WriteLine("3) Settings");
            Console.WriteLine("4) Exit");

            byte NumberOfOptions = 4;

            byte UserInput = Events.GetByteInputFromUser(NumberOfOptions);

            switch (UserInput)
            {
                case 1:
                    Console.WriteLine("1) Continue");
                    break;
                case 2:
                    StartGame();
                    break;
                case 3:
                    SettingsMenu();
                    break;
                case 4:
                    System.Environment.Exit(0);
                    break;
            }

        }

        public void SettingsMenu()
        {
            Console.Clear();
            List<Good> GoodsFromDatabase = ShowGoodsFromDatabase();

            Console.WriteLine("1) Add New Good");
            Console.WriteLine("2) Update Good");
            Console.WriteLine("3) Return To Main Menu");

            byte NumberOfOptions = 3;

            byte UserInput = Events.GetByteInputFromUser(NumberOfOptions);

            switch (UserInput)
            {
                case 1:
                    AddGoodsMenu();
                    break;
                case 2:
                    UpdateGoodsMenu(GoodsFromDatabase);
                    break;
                case 3:
                    MainMenu();
                    break;
            }
        }

        public List<Good> ShowGoodsFromDatabase()
        {
            List<Good> GoodsFromDatabase = GoodsModel.GetAllGoods();

            for (int i = 0; i < GoodsFromDatabase.Count; i++)
            {
                string name = GoodsFromDatabase[i].Name;
                byte weight = GoodsFromDatabase[i].Weight;
                int price = GoodsFromDatabase[i].Price;

                Console.WriteLine($"{i + 1}) {name} {weight} {price}");
            }
            Console.WriteLine(" ");
            return GoodsFromDatabase;
        }
        public void UpdateGoodsMenu(List<Good> goodsFromDatabase)
        {
            Console.WriteLine("Choose good to update");
            byte ChoosenGood = Events.GetByteInputFromUser((byte) goodsFromDatabase.Count);
            string ChoosenGoodName = goodsFromDatabase[ChoosenGood - 1].Name;

            Console.WriteLine("Write new weight of the good (max 255)");
            byte NewWeight = Events.GetByteInputFromUser(255);
            
            Console.WriteLine("Write new price of the good");
            int NewPrice = Events.GetIntegerInputFromUser(10000);

            GoodsModel.Update(ChoosenGoodName, NewWeight, NewPrice);

            SettingsMenu();
        }
        
        public void AddGoodsMenu() 
        {
            Console.WriteLine("Write name of the good");
            string Name = Console.ReadLine();
            if (Name == null) AddGoodsMenu();

            Console.WriteLine("Write weight of the good (max 255)");
            byte Weight = Events.GetByteInputFromUser(255);

            Console.WriteLine("Write new price of the good (max 10000)");
            int Price = Events.GetIntegerInputFromUser(10000);

            GoodsModel.Add(Name, Weight, Price);

            SettingsMenu();
        }
    }
}
