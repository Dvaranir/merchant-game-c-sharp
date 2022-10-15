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

        const int MaxRequiredGoods = 3;
        readonly string[] GoodsNames = new string[] { "Meat", "Fruits", "Paint", "Flour", "Seeds", "Cloth" };

        public Shop (List<Good> goods)
        {
            AllGoods = GenerateGoods().OrderBy(good => good.Price).ToList();
            MinPrice = AllGoods.MinBy(good => good.Price)!.Price;
            MinWeight = AllGoods.MinBy(good => good.Weight)!.Weight;
            AllGoodsCount = AllGoods.Count();
        }

        private List<Good> GenerateGoods()
        {
            List<Good> goods = new();
            Array.ForEach(GoodsNames, name => goods.Add(new Good(name)));
            return goods;
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

        public Good? GetLowestPriceGood() =>
            AllGoods.MinBy(good => good.Price);
        
        public Good? GetLowestWeightGood() =>
            AllGoods.MinBy(good => good.Weight);

        public List <Good> ChooseGoodsForCustomerNeeds(double money, int spaceLeft) =>
            AllGoods.FindAll(good =>
            good.Price <= money &
            good.Weight <= spaceLeft);

        public Good GetGoodForCustomerNeeds(double money, int spaceLeft) =>
            GenerateRandomGood(ChooseGoodsForCustomerNeeds(money, spaceLeft));
    }
}
