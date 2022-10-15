using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Entities
{
    internal class Shop
    {
        public int MinPrice { get; set; }
        public int MinWeight { get; set; }
        public List<Good> AllGoods { get; set; }
        public int AllGoodsCount { get; set; }
        private Merchant Player { get; set; }

        const int MaxRequiredGoods = 3;

        public Shop (List<Good> goods, Merchant player)
        {
            AllGoods = goods.OrderBy(good => good.Price).ToList();
            MinPrice = AllGoods.MinBy(good => good.Price)!.Price;
            MinWeight = AllGoods.MinBy(good => good.Weight)!.Weight;
            AllGoodsCount = AllGoods.Count();
            Player = player;
        }

        public List<string> GenerateRequiredGoods()
        {
            List<string> RequiredGoods = new();
            int NumberOfRequiredGoods = Random.Shared.Next(MaxRequiredGoods);
            List<int> RandomNumbersBlacklist = new();
             
            while (true)
            {
                int RandomNumber = Random.Shared.Next(AllGoodsCount - 1);

                if (RandomNumbersBlacklist.Contains(RandomNumber)) continue;

                string RandomGoodName = AllGoods[RandomNumber].Name;

                RequiredGoods.Add(RandomGoodName);
                RandomNumbersBlacklist.Add(RandomNumber);

                if (RequiredGoods.Count() >= NumberOfRequiredGoods) break;
            } 

            return RequiredGoods;
        }

        public Good GenerateRandomGood() =>
            AllGoods[Random.Shared.Next(1, AllGoodsCount - 1)];

        public Good GenerateRandomGood(List<Good> goods) =>
            goods[Random.Shared.Next(1, AllGoodsCount - 1)];

        public Good GetGoodForCustomerNeeds() =>
            GenerateRandomGood(ChooseGoodsForCustomerNeeds()) ;

        public Good? GetLowestPriceGood() =>
            AllGoods.MinBy(good => good.Price);
        
        public Good? GetLowestWeightGood() =>
            AllGoods.MinBy(good => good.Weight);

        public List <Good> ChooseGoodsForCustomerNeeds() =>
            AllGoods.FindAll(good =>
            good.Price <= Player.Money &
            good.Weight <= Player.CartCapacity - Player.CarryingWeight);
        
    }
}
