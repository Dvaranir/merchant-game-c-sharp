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
        public string Id { get; set; }

        readonly int GoodMaxWeight = Settings.GoodMaxWeight;
        readonly int GoodMinWeight = Settings.GoodMinWeight;

        readonly int GoodMaxPrice = Settings.GoodMaxPrice;
        readonly int GoodMinPrice = Settings.GoodMinPrice;

        readonly float NormalQuality = Settings.GoodNormalQuality;

        public Good(string name)
        {
            Name = name;
            Quality = NormalQuality;
            QualityTag = "Best";
            Weight = (byte) Random.Shared.Next(GoodMinWeight, GoodMaxWeight);
            Price = Random.Shared.Next(GoodMinPrice, GoodMaxPrice);
            Id = Guid.NewGuid().ToString();
        }
        public Good(string name, byte weight, int price )
        {
            Name = name;
            Quality = NormalQuality;
            GenerateQualityTag();
            Weight = weight;
            Price = price;
            Id = Guid.NewGuid().ToString();
        }
        public Good(string name, float quality, byte weight, int price)
        {
            Name = name;
            Quality = quality;
            GenerateQualityTag();
            Weight = weight;
            Price = price;
            Id = Guid.NewGuid().ToString();
        }
        public Good(string name, float quality, string qualityTag, byte weight, int price, string id)
        {
            Name = name;
            Quality = quality;
            QualityTag = qualityTag;
            Weight = weight;
            Price = price;
            Id = id;
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
            GenerateQualityTag();
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

    public struct GoodNameAndPrice
    {
        public string Name;
        public float Price;
    }
}
