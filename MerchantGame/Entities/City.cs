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

        readonly int MaxDistance = Settings.CityMaxDistance;
        readonly int MinDistance = Settings.CityMinDistance;

        public City()
        {
            Name = "";
            Distance = 0;
            RequiredGoods = new();
        }
        public City(string name, List<string> requiredGoods)
        {
            Name = name;
            Distance = (byte) Random.Shared.Next(MinDistance, MaxDistance);
            RequiredGoods = requiredGoods;
        }
        public City(string name, byte distance, List<string> requiredGoods)
        {
            Name = name;
            Distance = distance;
            RequiredGoods = requiredGoods;
        }
    }
}
