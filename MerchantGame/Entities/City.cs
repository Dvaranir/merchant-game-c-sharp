using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerchantGame.Models;

namespace MerchantGame.Entities
{
    internal class City
    {
        public string Name { get; set; }
        public byte Distance { get; set; }
        public List<string> RequiredGoods { get; set; }

        const int MaxDistance = 101;
        const int MinDistance = 50;

        public City(string name)
        {
            Name = name;
            Distance = (byte) Random.Shared.Next(MinDistance, MaxDistance);
            RequiredGoods = new List<string>();
        }
        public City(string name, byte distance, List<string> requiredGoods)
        {
            Name = name;
            Distance = distance;
            RequiredGoods = requiredGoods;
        }
    }
}
