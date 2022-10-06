using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Entities
{
    internal class Good
    {
        public string Name { get; set; }
        public float Quality { get; set; }
        public string QualityDescription { get; set; }
        public byte Weight { get; set; }
        public int Price { get; set; }

        const int GoodMaxWeight = 10;
        const int GoodMinWeight = 1;

        const int GoodMaxPrice = 200;
        const int GoodMinPrice = 10;

        const float NormalQuality = 1.2f;

        public Good(string name)
        {
            Name = name;
            Quality = NormalQuality;
            GenerateQualityDescription();
            Weight = (byte)Random.Shared.Next(GoodMinWeight, GoodMaxWeight);
            Price = Random.Shared.Next(GoodMinPrice, GoodMaxPrice);
        }
        public Good(string name, float quality, byte weight, int price )
        {
            Name = name;
            Quality = quality;
            Weight = weight;
            Price = price;
        }

        public string GenerateQualityDescription() {
            switch (Quality)
            {
                case 1.2f:
                    return "Best";
                case 1f:
                    return "Normal";
                case 0.60f:
                    return "Slightly Spoiled";
                case 0.25f:
                    return "Half Spoiled";
                case 0.1f:
                    return "Almost Totally Spoiled";
            }
        }

        public void GoBad()
        {
            switch (Quality)
            {
                case 1.2f:
                    Quality = 1f;
                    break;
                case 1f:
                    Quality = 0.60f;
                    break ;
                case 0.60f:
                    Quality = 0.25f;
                    break;
                case 0.25f:
                    Quality = 0.1f;
                    break;
                case 0.1f:
                    Quality = 0;
                    break;
                default:
                    Name = $"Spoiled {Name}";
                    break;
            }
        }
    }
}
