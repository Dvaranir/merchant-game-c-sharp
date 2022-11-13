namespace MerchantGame.Entities
{

    internal class Events
    {
        public Merchant Player { get; set; }
        public Shop Shop { get; set; }
        public City[] AllCities { get; set; }
      
        Action[] AllEvents { get; set; }

        readonly int MaxGoodsToSteal = Settings.EventsMaxGoodsToSteal;
        readonly int NightInTavernPrice = Settings.EventsNightInTavernPrice;
        
        const byte No = 2;

        public Events(Merchant player, Shop shop, City[] allCities)
        {
            Player = player;
            Shop = shop;
            AllCities = allCities;
            AllEvents =
            new Action[] {
                NormalDay, SmoothRoad, CartIsBroken,
                River, MeetLocal, RoguesAttack, Rain,
                GoodRotten, RoadsideTavern
            };
        }

        public void NormalDay()
        {
            Player.SpeedUpAndRide();

            string EventAddition = "What a beautiful day";
            DayAnnouncement(EventAddition);
        }

        public void Rain()
        {
            GoodRotten();
            Player.SpeedUpAndRide(1, 3);

            string EventAddition = "It was raining all day";
            DayAnnouncement(EventAddition);
        }

        public void GoodRotten()
        {
            int NumberOfGoods = Player.GoodsInCart.Count;
            int RandomGoodIndex = Random.Shared.Next(NumberOfGoods - 1);
            Good GoodInCart = Player.GoodsInCart[RandomGoodIndex];
            GoodInCart.GoBad();
            Player.SpeedUpAndRide();

            string EventAddition = $"{GoodInCart.Name} has gone bad and now it's quality is {GoodInCart.QualityTag}";
            DayAnnouncement(EventAddition);
        }

        public void SmoothRoad()
        {
            Player.SpeedUpAndRide(3, 5);

            string EventAddition = "Such a nice road";
            DayAnnouncement(EventAddition);
        }

        public void CartIsBroken()
        {
            Player.Stay();

            string EventAddition = "My cart has broken. Was repairing it all day";
            DayAnnouncement(EventAddition);
        }

        public void River() 
        { 
            Player.SpeedUpAndRide(1, 2);

            string EventAddition = "Road ended at river, where is my swimming trunks, when I need them?";
            DayAnnouncement(EventAddition);
        }

        public void MeetLocal()
        {
            Player.SpeedUp();
            Player.CartSpeed += (byte) Random.Shared.Next(3, 6);
            Player.Ride();

            string EventAddition = "I meet local. Such a nice dude, he showed me shortcut";
            DayAnnouncement(EventAddition);
        }

        public void RoguesAttack()
        {
            string EventAddition;

            if (Player.Money > 100) { 
                TakeMoney();
                EventAddition = "Met some bastards, they took my money!";
            }
            else { 
                TakeGoods();
                EventAddition = "Met some bastards, they took some of my goods!";
            }
            Player.SpeedUpAndRide();

            DayAnnouncement(EventAddition);
        }

        public void TakeGoods()
        {
            byte RoguesStealAmount = (byte) Random.Shared.Next(1, MaxGoodsToSteal);

            for (byte i = RoguesStealAmount; i > 0; i--)
                Player.GiveAwayBestGood();
        }

        public void TakeMoney()
        {
            Player.Money = 0;
        }

        public void RoadsideTavern()
        {
            bool WillGossipsAppear = Random.Shared.Next(100) >= 50;
            StayInTavern();
            TradeInTavern();
            if (!Player.GossipsEventAppeared && WillGossipsAppear) HearGossips();
        }

        public void StayInTavern()
        {
            byte Stay;

            const int TavernAnnouncementDayModifier = 1;


            string EventAddition = "You saw Roadside Tavern";
            DayAnnouncement(EventAddition, TavernAnnouncementDayModifier);
            Console.WriteLine("Will you stay here?\n1 - Yes, I will stay\n2 - No, I won't");

            Stay = GetByteInputFromUser();

            if (Stay == No)
            {
                Player.SpeedUpAndRide();
                return;
            }

            Player.PayForTavern(NightInTavernPrice);
            Player.Stay();
        }

        public void TradeInTavern()
        {
            byte Trade;
            string[] TypesOfTrade = GetPossibleTrades();
            if (TypesOfTrade.Length == 1)
            {
                Console.WriteLine("\nLooks like you can't trade");
                return;
            }

            Console.WriteLine("\nWill you trade here?\n1 - Yes, I will trade\n2 - No, I won't");
            Trade = GetByteInputFromUser();
            Console.WriteLine();
            if (Trade == No) return;

            int TypeOfTradeIndex = Random.Shared.Next(TypesOfTrade.Length);
            string RandomTypeOfTrade = TypesOfTrade[TypeOfTradeIndex];

            Good GoodForPlayer;
            string Message = "";
            char MessageChar = '-';

            switch (RandomTypeOfTrade)
            {
                case "Sell":
                    GoodNameAndPrice NameAndPrice = Player.SellGood();
                    Message = $"{Player.Name} sold {NameAndPrice.Name} for {(int) NameAndPrice.Price}$";
                    break;

                case "Exchange":
                    GoodForPlayer = ChooseGoodForPlayer();
                    string GoodName = GoodForPlayer.Name;
                    string ExchangedGoodName = Player.ExchangeGood(GoodForPlayer);
                    Message = $"{Player.Name} exchanged {ExchangedGoodName} on {GoodName}";
                    break;

                case "Buy":
                    GoodForPlayer = ChooseGoodForPlayer();
                    Player.BuyGood(GoodForPlayer);
                    Message = $"{Player.Name} bought {GoodForPlayer.Name}";
                    break;       
            }

            FormatAnnouncement(Message, MessageChar);
        }

        public void HearGossips()
        {
            Player.GossipsEventAppeared = true;
            string MainMessage = "Your heared gossip in tavern.";
            char MainMessageChar = '-';
            FormatAnnouncement(MainMessage, MainMessageChar);

            City RandomCity = AllCities[Random.Shared.Next(AllCities.Length)];
            string CityRequiredGoods = string.Join(", ", RandomCity.RequiredGoods);
            string Gossip = $"People says that {RandomCity.Name} in need of {CityRequiredGoods}";
            char GossipChar = '!';
            FormatAnnouncement(Gossip, GossipChar);

            int PossibleProfitInCurrentCity = Shop.CalculatePossibleProfit(Player);
            int PossibleProfitInNewCity = Shop.CalculatePossibleProfit(Player, RandomCity);

            string Choice = $"Now you are moving to {Player.DestinationCity.Name} and will possibly earn {PossibleProfitInCurrentCity}$\nBut in {RandomCity.Name} possible earnings may be {PossibleProfitInNewCity}$";
            char ChoiceChar = '$';
            FormatAnnouncement(Choice, ChoiceChar);

            Console.WriteLine($"Would you like to change destination city?\n1 - Yes, I will move to new city\n2 - No, I'l stay on my way");

            byte ChangeCity = GetByteInputFromUser();

            if (ChangeCity == No) return;

            Player.ChangeDestinationCity(RandomCity);
            
        }
        
        public string[] GetPossibleTrades()
        {
            int MinPrice = Shop.MinPrice;
            int MinWeight = Shop.MinWeight;

            if (Player.CartCapacity >= MinWeight &&
                Player.Money >= MinPrice)
                return new string[] { "Buy", "Sell", "Exchange" };
            else if (Player.GoodsInCart.Count > 0)
                return new string[] { "Sell", "Exchange" };
            else return new string[] { "No trades for you" };
        }

        public static byte GetByteInputFromUser(byte numberOfOptions = 2)
        {
            byte Output;
            while (true)
            {
                try
                {
                    Output = byte.Parse(Console.ReadLine());
                    if (Output > numberOfOptions || Output < 0) throw new Exception();
                    break;
                }
                catch
                {
                    Console.WriteLine($"Wrong input, please type a number in range from 1 to {numberOfOptions}");
                }
            }
            
            return Output;
        }
       
        public static int GetIntegerInputFromUser(int maxValue = 10_000, int minValue = 0)
        {
            string Message = $"Please type a number in range from {minValue} to {maxValue}";
            Console.WriteLine(Message);

            int Output;
            while (true)
            {
                try
                {
                    Output = int.Parse(Console.ReadLine());
                    if (Output > maxValue || Output < minValue) throw new Exception();
                    break;
                }
                catch
                {
                    Console.WriteLine($"Wrong input. {Message}");
                }
            }
            
            return Output;
        }
        public static string GetStringInputFromUser(int maxValue = 100, int minValue = 1)
        {
            string Message = $"Write a word or a sentence with minimum {minValue} and maximum {maxValue} characters";
            string Output;
            while (true)
            {
                try
                {
                    Output = Console.ReadLine();
                    if (Output.Length > maxValue || Output.Length < minValue) throw new Exception();
                    break;
                }
                catch
                {
                    Console.WriteLine($"Wrong input. {Message}");
                }
            }
            
            return Output;
        }

        public void DayAnnouncement(string EventAddition)
        {
            string AnounceString = $"Day №{Player.DaysOnRoad} {EventAddition}";
            FormatAnnouncement(AnounceString);
        }
            
        
        public void DayAnnouncement(string EventAddition, int DayModifier)
        {
            string AnounceString = $"Day №{Player.DaysOnRoad + DayModifier} {EventAddition}";
            FormatAnnouncement(AnounceString);
        }
            
        public void FormatAnnouncement(string message, char symbol = '*', int amountOfChars = 3)
        {
            string[] MessageList = message.Split("\n");
            string SideChars = new(symbol, amountOfChars);
            byte NumberOfSidechars = 2;
            byte NumberOfSpaces = 2;
            int LengthModifier = SideChars.Length * NumberOfSidechars;

            int SymbolStringLength;
            string SymbolString, FormattedMessage;

            if (MessageList.Length > 1)
            {
                SymbolStringLength = MessageList.MaxBy(message => message.Length).Length + LengthModifier + NumberOfSpaces;
                SymbolString = new(symbol, SymbolStringLength);

                Console.WriteLine(SymbolString);
                foreach (string line in MessageList)
                {
                    string PreformatedLine = $" {line} ";
                    FormattedMessage = PadCenter(PreformatedLine, SymbolStringLength, symbol);
                    Console.WriteLine(FormattedMessage);
                }
                Console.WriteLine(SymbolString);
                Console.WriteLine();
            }

            else 
            {
                string PreFormattedMessage = $" {message} ";
                SymbolStringLength = PreFormattedMessage.Length + LengthModifier;
                FormattedMessage = PadCenter(PreFormattedMessage, SymbolStringLength, symbol);
                SymbolString = new(symbol, SymbolStringLength);

                Console.WriteLine(SymbolString);
                Console.WriteLine(FormattedMessage);
                Console.WriteLine(SymbolString);
                Console.WriteLine();
            }
        }

        public string PadCenter(string source, int length, char symbol = '*')
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft, symbol).PadRight(length, symbol);
        }

        public Good ChooseGoodForPlayer() =>
            Shop.GetGoodForCustomerNeeds(Player.Money, Player.CartCapacity - Player.CartCapacity);

        public void RandomEvent() =>
            AllEvents[Random.Shared.Next(AllEvents.Length)]();

    }
}
