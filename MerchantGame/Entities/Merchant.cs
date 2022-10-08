using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Entities
{
    internal class Merchant
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public byte CartSpeed { get; set; }
        public int CartCapacity { get; set; }
        public int CarryingWeight { get; set; }
        public List<Good> GoodsInCart { get; set; }
        public byte DistanceLeft { get; set; } 

        const int MaxMoney = 5000;

        public Merchant(string name = "Player")
        {
            Name = name;
            CartCapacity = 2000;
            CarryingWeight = 0;
            Money = Random.Shared.Next(MaxMoney);
            GoodsInCart = new List<Good>();
        }

        public void SpeedUp(byte minSpeed = 1, byte maxSpeed = 5)
        {
            CartSpeed = (byte) Random.Shared.Next(minSpeed, maxSpeed);
        }

        public void RideNext()
        {
            DistanceLeft -= CartSpeed;
        }

        public void SpeedUpAndRide(byte minSpeed = 1, byte maxSpeed = 5)
        {
            SpeedUp(minSpeed, maxSpeed);
            RideNext();
        }

        //public void BuyGood(List<Good> goods) {
        //    int GoodsLength = goods.Count;
        //    Good RandomGood = goods[Randomizer.Next(GoodsLength - 1)];
        //    if (
        //        MinWeight + CarryingWeight > CartCapacity ||
        //        Money - MinPrice <= 0
        //        ) return;


        //    GoodsInCart.Add(RandomGood);
        //}
    }
}
