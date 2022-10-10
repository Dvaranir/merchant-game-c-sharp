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

        const int MaximumMoney = 5000;

        public Merchant(string name = "Player")
        {
            Name = name;
            CartCapacity = 2000;
            CarryingWeight = 0;
            Money = Random.Shared.Next(MaximumMoney);
            GoodsInCart = new List<Good>();
        }

        public void SpeedUp(byte minSpeed = 1, byte maxSpeed = 5) =>
            CartSpeed = (byte) Random.Shared.Next(minSpeed, maxSpeed);
        
        public void Ride() => DistanceLeft -= CartSpeed;

        public void BuyGood(Good good) => GoodsInCart.Add(good);

        public void SpeedUpAndRide(byte minSpeed = 1, byte maxSpeed = 5)
        {
            SpeedUp(minSpeed, maxSpeed);
            Ride();
        }

        

    }
}
