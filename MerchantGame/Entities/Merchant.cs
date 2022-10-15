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
        public string DestinationCityName { get; set; }
        public byte DistanceLeft { get; set; } 
        public byte DaysOnTheRoad { get; set; }

        const int CartCapacitySetting = 2000;
        const int MaximumMoney = 5000;

        public Merchant(string name, 
                        byte distanceLeft,
                        string destinationCityName)
        {
            Name = name;
            CartCapacity = CartCapacitySetting;
            CarryingWeight = 0;
            Money = Random.Shared.Next(MaximumMoney);
            GoodsInCart = new List<Good>();
            DistanceLeft = distanceLeft;
            DestinationCityName = destinationCityName;
            DaysOnTheRoad = 0;
        }

        public void SpeedUp(byte minSpeed = 1, byte maxSpeed = 5) =>
            CartSpeed = (byte) Random.Shared.Next(minSpeed, maxSpeed);

        public void Ride()
        {
            DistanceLeft -= CartSpeed;
            DaysOnTheRoad++;
        }

        public void SpeedUpAndRide(byte minSpeed = 1, byte maxSpeed = 5)
        {
            SpeedUp(minSpeed, maxSpeed);
            Ride();
        }

        public int SelectRandomGoodInCart() =>
            Random.Shared.Next(GoodsInCart.Count - 1);
        

        public void BuyGood(Good good) => GoodsInCart.Add(good);

        public void SellGood(int goodIndex = 0, int sellModifier = 1)
        {
            if (GoodsInCart.Count <= 0) return;
            Good GoodToSell = GoodsInCart[goodIndex];
            Money += (GoodToSell.Price * GoodToSell.Quality) * sellModifier;
            GoodsInCart.RemoveAt(goodIndex);
        }

        public void ExchangeGood(Good newGood, int indexOfGood = 0) =>
            GoodsInCart[indexOfGood] = newGood;
        

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
