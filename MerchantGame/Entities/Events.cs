using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Entities
{
    
    internal class Events
    {
        public Merchant Player { get; set; }
        public Shop Shop { get; set; }

        const int MaxGoodsToSteal = 5;
        const int NightInTavernPrice = 50;

        public Events(Merchant player, Shop shop)
        {
            Player = player;
            Shop = shop;
        }

        public void NormalDay() => Player.SpeedUpAndRide();

        public void Rain()
        {
            GoodRotten();
            Player.SpeedUpAndRide(1, 3);
        }

        public void GoodRotten()
        {
            int NumberOfGoods = Player.GoodsInCart.Count;
            int RandomGoodIndex = Random.Shared.Next(NumberOfGoods - 1);
            Player.GoodsInCart[RandomGoodIndex].GoBad();
        }

        public void SmoothRoad() => Player.SpeedUpAndRide(3, 5);

        public void CartIsBroken() => Player.SpeedUpAndRide(0, 0);

        public void River() => Player.SpeedUpAndRide(1, 2);

        public void MeetLocal()
        {
            Player.SpeedUp();
            Player.CartSpeed += (byte) Random.Shared.Next(3, 6);
            Player.Ride();
        }

        public void RoguesAttack()
        {
            if (Player.Money > 100) TakeMoney();
            else TakeGoods();
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
            byte Stay, Trade;
            const byte No = 1;

            Console.WriteLine("You saw Roadside Tavern, will you stay here?\n1 - Yes, I will stay\n2 - No, I won't");
            Stay = GetInputFromUser();
            if (Stay == No) return;

            Player.PayForTavern(NightInTavernPrice);

            Console.WriteLine("Will you trade here?\n1 - Yes, I will trade\n2 - No, I won't");
            Trade = GetInputFromUser();
            if (Trade == No) return;

            string[] TypesOfTrade = {"Buy", "Sell", "Exchange"};

            int TypeOfTradeIndex = Random.Shared.Next(TypesOfTrade.Length - 1);
            string RandomTypeOfTrade = TypesOfTrade[TypeOfTradeIndex];

            switch (RandomTypeOfTrade)
            {
                case "Sell":
                    Player.SellGood();
                    break;
                case "Exchange":
                    Player.ExchangeGood(Shop.GetGoodForCustomerNeeds());
                    break;
                case "Buy":
                    Player.BuyGood(Shop.GetGoodForCustomerNeeds());
                    break;
            }
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

        public byte GetInputFromUser()
        {
            byte Output;
            while (true)
            {
                try
                {
                    Output = byte.Parse(Console.ReadLine());
                    if (Output == 1 || Output == 2) break;
                }
                catch
                {
                    Console.WriteLine("Wrong answer, please type 1 or 2");
                }
            }
            
            return Output;
        }


    }
}
