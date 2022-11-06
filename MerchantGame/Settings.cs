using MerchantGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame
{
    internal static class Settings
    {
        public const int MerchantCartCapacity = 2000;
        public const int MerchantMaximumMoney = 5000;

        public const int CityMaxDistance = 101;
        public const int CityMinDistance = 50;

        public const int GoodMaxWeight = 10;
        public const int GoodMinWeight = 1;
        public const int GoodMaxPrice = 200;
        public const int GoodMinPrice = 10;
        public const float GoodNormalQuality = 1.2f;

        public const int EventsMaxGoodsToSteal = 5;
        public const int EventsNightInTavernPrice = 50;

        public const int ShopCityMaxRequiredGoods = 3;


        public enum GoodsNames { Meat, Fruits, Paint, Flour, Seeds, Cloth };

        public static string[] GetGoodsNames() =>
            Enum.GetNames(typeof(GoodsNames));


    }
}
