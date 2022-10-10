﻿using System;
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

        public Shop (List<Good> goods)
        {
            AllGoods = goods;
            MinPrice = goods.MinBy(good => good.Price)!.Price;
            MinWeight = goods.MinBy(good => good.Weight)!.Weight;
            AllGoodsCount = goods.Count();
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
    }
}
