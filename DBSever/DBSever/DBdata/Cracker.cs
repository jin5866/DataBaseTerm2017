using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DBdata
{
    class Cracker
    {
        /// <summary>
        /// _size 는 g단위로
        /// _name 은 영어로
        /// </summary>
        /// <param name="_UniqueNumber"></param>
        /// <param name="_Name"></param>
        /// <param name="_Size"></param>
        public Cracker(int _UniqueNumber, string _Name, int _Size)
        {
            UniqueNumber = _UniqueNumber;
            Name = _Name;
            Size = _Size;
        }
        public Cracker()
        {

        }
        public int UniqueNumber{ get; set; }
        public string Name{ get; set; }

        public int Size{ get; set; }
    }
}
