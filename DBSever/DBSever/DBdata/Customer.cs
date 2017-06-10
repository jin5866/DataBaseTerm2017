using Server;
using Server.DBdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServer.DBdata
{
    class Customer
    { 
        public Customer() { }

        public int UniqueNumber { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public DateTime JoinTime { get; set; }


        

        
        public int GetPreference(Cracker cracker)
        {
            return Core.GetPreference(this.UniqueNumber, cracker.UniqueNumber);
        }
        
    }
}
