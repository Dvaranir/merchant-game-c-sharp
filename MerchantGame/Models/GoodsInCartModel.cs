using MerchantGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Models
{
    internal class GoodsInCartModel : Model
    {
        public static void Add(List<Good> goodsInCart, string playerName)
        {
            StringBuilder stringBuilder = new("INSERT OR REPLACE INTO goods_in_cart VALUES ");
            foreach (Good good in goodsInCart)
            {
                stringBuilder.Append($"('{good.Id}', '{good.Name}', '{good.Quality}', '{good.QualityTag}', '{good.Weight}', '{good.Price}', '{playerName}'), ");
            }
            string Request = stringBuilder.ToString();
            Request = Request.Substring(0, Request.Length - 2) + ';';
            
            ExecuteRequest(Request);
        }
    }
}
