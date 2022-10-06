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
        public List<string> GoodsInDemand { get; set; }
        

        const int MaxDistance = 101;
        const int MinDistance = 50;


        public City(string name)
        {
            Name = name;
            Distance = (byte) Random.Shared.Next(MinDistance, MaxDistance);
        }
        public City(string name, byte distance)
        {
            Name = name;
            Distance = distance;
        }
    }
}
