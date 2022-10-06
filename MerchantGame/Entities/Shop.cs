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
        public List<string> RequiredGoods { get; set; }

        public Shop (List<Good> goods)
        {
            AllGoods = goods;
            MinPrice = AllGoods.MinBy(good => good.Price).Price;
            MinWeight = AllGoods.MinBy(good => good.Weight).Weight;
        }
    }
}
