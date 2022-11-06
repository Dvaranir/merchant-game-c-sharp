﻿using System;
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

        Action[] AllEvents { get; set; }

        readonly int MaxGoodsToSteal = Settings.EventsMaxGoodsToSteal;
        readonly int NightInTavernPrice = Settings.EventsNightInTavernPrice;


        public Events(Merchant player, Shop shop)
        {
            Player = player;
            Shop = shop;
            AllEvents =
            new Action[] {
                NormalDay, SmoothRoad, CartIsBroken,
                River, MeetLocal, RoguesAttack, RoadsideTavern
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
            Player.GoodsInCart[RandomGoodIndex].GoBad();
            Player.SpeedUpAndRide();

            string EventAddition = "One product has gone bad";
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
                EventAddition = "Meet some bastards, they took my money!";
            }
            else { 
                TakeGoods();
                EventAddition = "Meet some bastards, they took some of my goods!";
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
            byte Stay, Trade;
            const byte No = 2;
            const int TavernAnnouncementDayModifier = 1;

            
            string EventAddition = "You saw Roadside Tavern, will you stay here?\n1 - Yes, I will stay\n2 - No, I won't";
            DayAnnouncement(EventAddition, TavernAnnouncementDayModifier);

            Stay = GetInputFromUser();

            if (Stay == No) 
            {
                Player.SpeedUpAndRide();
                return; 
            }

            Player.PayForTavern(NightInTavernPrice);
            Player.Stay();

            string[] TypesOfTrade = GetPossibleTrades();
            if (TypesOfTrade.Length == 1)
            {
                Console.WriteLine("Looks like you can't trade");
                return;
            }

            Console.WriteLine("Will you trade here?\n1 - Yes, I will trade\n2 - No, I won't");
            Trade = GetInputFromUser();
            if (Trade == No) return;

            int TypeOfTradeIndex = Random.Shared.Next(TypesOfTrade.Length - 1);
            string RandomTypeOfTrade = TypesOfTrade[TypeOfTradeIndex];

            Good GoodForPlayer;

            switch (RandomTypeOfTrade)
            {
                case "Sell":
                    GoodNameAndPrice NameAndPrice = Player.SellGood();
                    Console.WriteLine($"{Player.Name} sold {NameAndPrice.Name} for {NameAndPrice.Price}");
                    break;

                case "Exchange":
                    GoodForPlayer = ChooseGoodForPlayer();
                    string GoodForPlayerName = GoodForPlayer.Name;
                    string ExchangedGoodName = Player.ExchangeGood(GoodForPlayer);
                    Console.WriteLine($"{Player.Name} exchanged {ExchangedGoodName} on {GoodForPlayerName}");
                    break;

                case "Buy":
                    GoodForPlayer = ChooseGoodForPlayer();
                    Player.BuyGood(GoodForPlayer);
                    Console.WriteLine($"{Player.Name} bought {GoodForPlayer.Name}");
                    break;

                default:
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

        public static byte GetInputFromUser()
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

        public void DayAnnouncement(string EventAddition) =>
            Console.WriteLine($"Day N{Player.DaysOnTheRoad} {EventAddition}");
        public void DayAnnouncement(string EventAddition, int DayModifier) =>
            Console.WriteLine($"Day N{Player.DaysOnTheRoad + DayModifier} {EventAddition}");

        public Good ChooseGoodForPlayer() =>
            Shop.GetGoodForCustomerNeeds(Player.Money, Player.CartCapacity - Player.CartCapacity);

        public void RandomEvent() =>
            AllEvents[Random.Shared.Next(AllEvents.Length)]();


    }
}
