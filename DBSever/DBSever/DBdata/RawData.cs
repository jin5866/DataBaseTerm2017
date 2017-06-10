using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServer.DBdata
{
    class RawData
    {
        public RawData(byte[] item)
        {
            if (item.Length < 13)
                return;
            if (item[0] > 3)
                return;
            DataType = (RawDataType)Enum.ToObject(typeof(RawDataType), item[0]);

            EatTime = BitConverter.ToInt32(item, 1);
            CrackerUN = BitConverter.ToInt32(item, 5);
            CustomerUN = BitConverter.ToInt32(item, 9);

        }

        public RawData()
        {

        }

        override public string ToString()
        {

            return "DataType:" + DataType + "\n" +
                "EatTime:" + EatTime + "\n" +
                "CustomerUN: " + CustomerUN + "\n" +
                "CrackerUN: " + CrackerUN;
        }

        public RawDataType DataType { get; set; }

        /// <summary>
        /// int 값에서
        /// 앞에서부터 1바이트씩 Day Time Minute Second 입니다.
        /// </summary>
        public int EatTime { get; set; }

        public int CrackerUN { get; set; }

        public int CustomerUN { get; set; }






        public bool SetEatTime(int Day,int Time,int Minute,int Second)
        {
            if (Day > 32)
                return false;
            if (Time > 24)
                return false;
            if (Minute > 60)
                return false;
            if (Second > 60) 
                return false;

            EatTime = (Day << 24) + (Time << 16) + (Minute << 8) + Second;


            return true;
    
        }

        /// <summary>
        /// a - b 
        /// Day Time Minute Second
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public int SubEatTime(int a, int b)
        {
            byte[] aa = BitConverter.GetBytes(a);
            byte[] bb = BitConverter.GetBytes(b);

            byte[] carry = { 0, 0, 0, 0 };
            byte[] unit = { 60, 60, 24, (byte)Setting.BEFORE_MONTH_DATE };


            for (int i = 0; i < 4; i++)
            {
                if (aa[i] < (bb[i] + carry[1]))
                {
                    aa[i] += unit[i];
                    if (i != 4)
                    {
                        carry[i + 1] += 1;
                    }
                }

                aa[i] -= (byte)(bb[i] + carry[i]);
            }


            return BitConverter.ToInt32(aa, 0);
            
        }

        static public int EatTimeToSecond(int eatTime)
        {
            byte[] a = BitConverter.GetBytes(eatTime);

            return a[0] + a[1] * 60 + a[2] * 60 * 60 + a[3] * 60 * 60 * 24;
        }

        static public DateTime timeToDateTime(int time)
        {
            int Year = 0;
            int Month = 0;
            
            byte[] times = BitConverter.GetBytes(time);
            DateTime tmp;
            //달을 넘어간경우
            if (DateTime.Now.Day > times[3])
            {
                //년을 넘어간 경우
                if(DateTime.Now.Month == 1)
                {
                    Year = DateTime.Now.Year - 1;
                    Month = 12;
                }
                else
                {
                    Year = DateTime.Now.Year;
                    Month = DateTime.Now.Month - 1;
                }
            }
            else
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
            }

            tmp = new DateTime(Year, Month, times[3], times[2], times[1], times[0]);

            return tmp;
        }
    }

    
}
