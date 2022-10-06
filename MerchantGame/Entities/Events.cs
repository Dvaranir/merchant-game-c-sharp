using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MerchantGame.Entities
{
    
    internal class Events
    {
        public Merchant Player { get; set; }

        public Events(Merchant player)
        {
            Player = player;
        }
    }
}
