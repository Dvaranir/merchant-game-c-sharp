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
        public string? QualityTag { get; set; }
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
            QualityTag = "Best";
            Weight = (byte)Random.Shared.Next(GoodMinWeight, GoodMaxWeight);
            Price = Random.Shared.Next(GoodMinPrice, GoodMaxPrice);
        }
        public Good(string name, float quality, byte weight, int price )
        {
            Name = name;
            Quality = quality;
            GenerateQualityTag();
            Weight = weight;
            Price = price;
        }

        public void GoBad()
        {
            switch (Quality)
            {
                case 1.2f:
                    Quality = 0.95f;
                    QualityTag = "Normal";
                    break;
                case 0.95f:
                    Quality = 0.55f;
                    QualityTag = "Half Spoiled";
                    break ;
                case 0.55f:
                    Quality = 0.25f;
                    QualityTag = "Almost Totally Spoiled";
                    break;
                default:
                    Quality = 0.1f;
                    QualityTag = "Spoiled";
                    break;
            }
        }

        public void GenerateQualityTag()
        {
            switch (Quality)
            {
                case 1.2f:
                    QualityTag = "Best";
                    break;
                case 0.95f:
                    QualityTag = "Normal";
                    break;
                case 0.55f:
                    QualityTag = "Half Spoiled";
                    break;
                case 0.25f:
                    QualityTag = "Almost Totally Spoiled";
                    break;
                default:
                    QualityTag = "Spoiled";
                    break;
            }
        }
    }
}
