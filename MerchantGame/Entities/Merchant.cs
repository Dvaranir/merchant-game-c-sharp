using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Entities
{

    internal class Merchant
    {
        public string Name { get; set; }
        public double Money { get; set; }
        public byte CartSpeed { get; set; }
        public int CartCapacity { get; set; }
        public int CarryingWeight { get; set; }
        public List<Good> GoodsInCart { get; set; }
        public string StartingCityName { get; set; }
        public string DestinationCityName { get; set; }
        public int DistanceLeft { get; set; }
        public int DaysOnTheRoad { get; set; }

        readonly int MaximumMoney = Settings.MerchantMaximumMoney;
        readonly int CartCapacitySetting = Settings.MerchantCartCapacity;

        public Merchant(string startingCityName, City destinationCity, string name = "Player 1")
        {
            Name = name;
            CartCapacity = CartCapacitySetting;
            CarryingWeight = 0;
            Money = Random.Shared.Next(MaximumMoney);
            GoodsInCart = new List<Good>();
            StartingCityName = startingCityName;
            DestinationCityName = destinationCity.Name;
            DistanceLeft = destinationCity.Distance;
            DaysOnTheRoad = 0;
        }
        public Merchant(string name,
                        byte distanceLeft,
                        string startingCityName,
                        string destinationCityName)
        {
            Name = name;
            CartCapacity = CartCapacitySetting;
            CarryingWeight = 0;
            Money = Random.Shared.Next(MaximumMoney);
            GoodsInCart = new List<Good>();
            DistanceLeft = distanceLeft;
            StartingCityName = startingCityName;
            DestinationCityName = destinationCityName;
            DaysOnTheRoad = 0;
        }

        

        public void SpeedUp(byte minSpeed = 1, byte maxSpeed = 5) =>
            CartSpeed = (byte)Random.Shared.Next(minSpeed, maxSpeed);

        public void Ride()
        {
            DistanceLeft -= CartSpeed;
            DaysOnTheRoad++;
        }

        public void Stay() => DaysOnTheRoad++;

        public void SpeedUpAndRide(byte minSpeed = 1, byte maxSpeed = 5)
        {
            SpeedUp(minSpeed, maxSpeed);
            Ride();
        }

        public int SelectRandomGoodInCart() =>
            Random.Shared.Next(GoodsInCart.Count - 1);


        public void BuyGood(Good good) 
        {
            GoodsInCart.Add(good);
            Money -= good.Price;
        }

        public GoodNameAndPrice SellGood(int goodIndex = 0, int sellModifier = 1)
        {
            GoodNameAndPrice NameAndPrice;
            if (GoodsInCart.Count <= 0) return new GoodNameAndPrice();

            Good GoodToSell = GoodsInCart[goodIndex];
            NameAndPrice.Name = GoodToSell.Name;

            float SellPrice = (GoodToSell.Price * GoodToSell.Quality) * sellModifier;
            NameAndPrice.Price = SellPrice;

            Money += SellPrice;
            GoodsInCart.RemoveAt(goodIndex);

            return NameAndPrice;
        }

        public string ExchangeGood(Good newGood, int indexOfGood = 0) 
        {
            string ExchangedGoodName = GoodsInCart[indexOfGood].Name;
            GoodsInCart[indexOfGood] = newGood;
            return ExchangedGoodName;
        }
            
        

        public void GiveAwayBestGood()
        {
            if (GoodsInCart.Count > 0) return;
            try {
                Good GoodWithMaxPrice = GoodsInCart.MaxBy(good => good.Price);
                GoodsInCart.Remove(GoodWithMaxPrice);
            } catch { }
        }

        public void PayForTavern(int overnightCost)
        {
            if (Money >= overnightCost) Money -= overnightCost;
            else GiveAwayBestGood();
        }

    }
    
}
