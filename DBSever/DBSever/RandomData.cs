using DBServer.DBdata;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSever
{
    static class RandomData
    {
        static DBConnecter connecter = new DBConnecter();
        static Random a = new Random();

        public static void AddRandomCustomer(int max)
        {
            var sql = "Insert Customer values ('{0}','{1}','{2}', '{3}')";
            int now;
            try
            {
                now = connecter.GetList<Customer>("select * from Customer where UniqueNumber = (Select max(UniqueNumber) from Customer)")[0].UniqueNumber + 1;
            }
            catch(Exception e)
            {
                now = 0;
            }
            
            int crackerMax = connecter.GetList<int>("Select * From Cracker Where UniqueNumber = (Select max(UniqueNumber) from Cracker)")[0];

            for (;now<=max;now++)
            {
                var tmp = RandomCustomer(now);
                connecter.Insert(tmp);
                //connecter.SendSQL(String.Format(sql,tmp.UniqueNumber,tmp.Name,tmp.ID,tmp.JoinTime.ToString("yyyy-MM-dd HH:mm:ss")));
                for(int i = 0;i<=crackerMax;i++)
                {
                    connecter.Insert(RandomRel(tmp, i));
                }

            }
        }
        static CusCraRelation RandomRel(Customer Cus, int CraUN)
        {
            CusCraRelation tmp = new CusCraRelation();

            tmp.CrackerUN = CraUN;
            tmp.CustomerUN = Cus.UniqueNumber;
            int rd = a.Next(0, (DateTime.Now - Cus.JoinTime).Days);

            tmp.FirstEat = Cus.JoinTime.AddDays(rd);
            tmp.EatingTimeAvg = 0;
            tmp.EatingTermAvg = 0;
            tmp.Preference = 0;
            tmp.EatingCount = a.Next(0, 100);

            return tmp;
        }


        static Customer RandomCustomer( int UN)
        {
            Customer tmp = new Customer();

            tmp.ID = GetRandomString(a, 10);

            int year = 2015 + a.Next(0, 2);
            int month = a.Next(1, 12);
            int day = a.Next(1, 28);
            int time = a.Next(1, 24);
            int minute = a.Next(1, 60);
            int second = a.Next(0, 60);
            tmp.JoinTime = new DateTime(year, month, day, time, minute, second);

            tmp.Name = GetRandomString(a, 6);

            tmp.UniqueNumber = UN;

            return tmp;
        }
        static Customer RandomCustomer()
        {
            return RandomCustomer(connecter.GetList<int>("Select max(UniqueNumber) from Customer")[0] + 1); 
        }

        static string GetRandomString(Random rnd, int length)
        {
            string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            return GetRandomString(rnd, length, charPool);
        }
        static string GetRandomString(Random rnd, int length, string charPool)
        {
            StringBuilder rs = new StringBuilder();

            while (length-- !=0)  
                rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

            return rs.ToString();
        }
    }
}
