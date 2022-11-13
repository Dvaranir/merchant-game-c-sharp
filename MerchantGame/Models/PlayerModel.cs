using MerchantGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Models
{
    internal class PlayerModel : Model
    {
        public static void Add(Merchant player)
        {
            string DestinationCityName = player.DestinationCity.Name;

            string Request = $"INSERT OR REPLACE INTO merchant VALUES ('{player.Name}', '{player.CartCapacity}', '{player.CarryingWeight}', '{player.Money}', '{player.StartingMoney}', '{player.DistanceLeft}', '{player.DistanceTraveled}', '{player.StartingCityName}', '{DestinationCityName}', '{player.DaysOnTheRoad}', '{player.GossipsEventAppeared}')";

            ExecuteRequest(Request);
        }
    }
}
