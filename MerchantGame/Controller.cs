using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerchantGame.Entities;
using MerchantGame.Models;

namespace MerchantGame
{
    internal class Controller
    {
        public List<City> Cities { get; set;}
        public City StartingCity { get; set; }
        public City DestinationCity { get; set; }
        public Events Events { get; set; }
        public Merchant Player { get; set; }

        public Controller (Events events, Merchant player)
        {
            Cities = CitiesModel.GetAllCities();
            
            StartingCity = ChooseRandomCity();
            do { DestinationCity = ChooseRandomCity(); }
            while (DestinationCity.Name == StartingCity.Name);
            
            Events = events;
            Player = player;
        }

        public City ChooseRandomCity() =>
            Cities[Random.Shared.Next(0, Cities.Count - 1)];


        
        


    }
}
