using Server;
using Server.DBdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServer.DBdata
{
    class CusCraRelation
    {
        public int CustomerUN { get; set; }

        public int CrackerUN { get; set; }

        public int Preference { get; set; }

        public int EatingCount { get; set; }

        //먹는 빈도
        public int EatingTimeAvg { get; set; }

        //먹는대 걸린 시간
        public int EatingTermAvg { get; set; }

        public DateTime FirstEat { get; set; }





    }
}
