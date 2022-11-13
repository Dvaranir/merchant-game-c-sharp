using System.Text;
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

            Cities = CitiesModel.GetAll();
            DestinationCity = GetRandomCity();
            Player = new(GetRandomCityName(), DestinationCity);
            Shop = new();
            Events = new(Player, Shop, Cities.ToArray());
        }

        public void StartGame()
        {
            Console.Clear();
            MerchantModel.Drop();
            ChangePlayerStats();
            InitialPurchase();
            MainGamePlay();
        }

        public void ContinueGame()
        {
            LoadSaveGame();
            MainGamePlay();
        }

        public void MainGamePlay()
        {
            MainLoop();
            SellAllGoods();
            EndGameStatistic();
            MerchantModel.Drop();
        }

        public void LoadSaveGame()
        {
            Player = MerchantModel.Get();
            DestinationCity = Player.DestinationCity;
            SyncShop();
            Events.Player = Player;
        }

        public void InitialPurchase()
        {
            SyncShop();
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
            Console.WriteLine();
        }

        public void SyncShop()
        {
            Shop.AllGoods = Model.GetAllGoods();
            Shop.Init();
        }
        public void MainLoop()
        {
            int SaveInterval = Settings.SaveInterval;
            int i = 0;

            while (true)
            {
                Events.RandomEvent();
                i++;

                if (i >= SaveInterval)
                {
                    SavePlayer();
                    i = 0;
                }

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
            Console.WriteLine($"{Player.Name} had {(int) Player.StartingMoney}$ at start");
            Console.WriteLine($"{Player.Name} have {(int) Player.Money}$ now");
            if (StartingMoney < Money)
                Console.WriteLine($"{Player.Name} earned {(int) (Player.Money - Player.StartingMoney)}$");
            else
                Console.WriteLine($"{Player.Name} earned nothing");

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        public City GetRandomCity() =>
            Cities[Random.Shared.Next(0, Cities.Length)];

        public string GetRandomCityName() =>
            Cities[Random.Shared.Next(0, Cities.Length)].Name;

        public void ChangePlayerStats()
        {
            Console.WriteLine("Type a name for your merchant:");
            string Name = Events.GetStringInputFromUser();
            Console.Clear();
            
            Console.WriteLine("Type cart capacity:");
            int CartCapacity = Events.GetIntegerInputFromUser();
            Console.Clear();

            Console.WriteLine("Type starting money:");
            int StartingMoney = Events.GetIntegerInputFromUser();
            Console.Clear();


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

            Player.Name = Name;
            Player.CartCapacity = CartCapacity;
            Player.StartingMoney = StartingMoney;
            Player.Money = StartingMoney;
            Player.StartingCityName = Cities[StartingCityChoice - 1].Name;

            MerchantModel.Add(Player);
            Console.Clear();
        }

        public void MainMenu()
        {
            Console.Clear();
            bool SaveExist = MerchantModel.Check();
            Console.WriteLine("1) New Game");
            Console.WriteLine("2) Settings");
            Console.WriteLine("3) Exit");
            byte NumberOfOptions = 3;

            if (SaveExist) 
            { 
                Console.WriteLine("4) Continue");
                NumberOfOptions = 4;
            }

            byte UserInput = Events.GetByteInputFromUser(NumberOfOptions);

            switch (UserInput)
            {
                case 1:
                    StartGame();
                    break;
                case 2:
                    SettingsMenu();
                    break;
                case 3:
                    System.Environment.Exit(0);
                    break;
                case 4:
                    ContinueGame();
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
            Console.WriteLine();

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
            List<Good> GoodsFromDatabase = GoodsModel.Get();

            for (int i = 0; i < GoodsFromDatabase.Count; i++)
            {
                string name = GoodsFromDatabase[i].Name;
                byte weight = GoodsFromDatabase[i].Weight;
                int price = GoodsFromDatabase[i].Price;

                Console.WriteLine($"{i + 1}) {name} {weight}kg {price}$");
            }
            Console.WriteLine(" ");
            return GoodsFromDatabase;
        }
        
        public void UpdateGoodsMenu(List<Good> goodsFromDatabase)
        {
            Console.WriteLine("Choose good to update");
            byte ChoosenGood = Events.GetByteInputFromUser((byte) goodsFromDatabase.Count);
            string ChoosenGoodName = goodsFromDatabase[ChoosenGood - 1].Name;
            Console.WriteLine();

            Console.WriteLine("Write new weight of the good (max 255)");
            byte NewWeight = Events.GetByteInputFromUser(255);
            Console.WriteLine();

            Console.WriteLine("Write new price of the good");
            int NewPrice = Events.GetIntegerInputFromUser(10000);
            Console.WriteLine();

            GoodsModel.Update(ChoosenGoodName, NewWeight, NewPrice);

            SettingsMenu();
        }
        
        public void AddGoodsMenu() 
        {
            Console.Clear();
            Console.WriteLine("Write a name of the good");
            string Name = Console.ReadLine();
            if (Name == null) AddGoodsMenu();

            Console.Clear();
            Console.WriteLine("Write weight of the good (max 255)");
            byte Weight = Events.GetByteInputFromUser(255);

            Console.Clear();
            Console.WriteLine("Write new price of the good (max 10000)");
            int Price = Events.GetIntegerInputFromUser(10000);

            GoodsModel.Add(Name, Weight, Price);

            SettingsMenu();
        }
        
        public void SavePlayer()
        {
            MerchantModel.Add(Player);
            GoodsInCartModel.Add(Player.GoodsInCart, Player.Name);
        }
    }
}
