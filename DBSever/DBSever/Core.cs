using DBServer.DBdata;
using Server;
using Server.DBdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServer
{
    static class Core
    {

        public static int GetPreference(Customer customer, Cracker cracker)
        {
            return GetPreference(customer.UniqueNumber, cracker.UniqueNumber);
        }

        public static int GetPreference(int customerUN, int crackerUN)
        { 
            var sql = "Select * " +
                "From CusCraRelation" +
                "Where CustomerUN = {0} AND" +
                "CrackerUN = {1}";

            int preference = 0;

            DBConnecter connecter = new DBConnecter();

            var relationData = connecter.GetList<CusCraRelation>(String.Format(sql, customerUN, customerUN));

            foreach(var rd in relationData)
            {
                preference = GetPreference(rd);
            }

            connecter.Close();
            
            

            return preference;
        }

        static public int GetPreference(CusCraRelation rd)
        {
            int[] dateGap = { 1, 7, 15, 30, 100 };
            int[] preferFromDate = { 100, 80, 60, 40, 20, 0 };

            int preference = 0;

            bool isGetted = false;
            float gapAvg = (float)(DateTime.Now - rd.FirstEat).Days / (rd.EatingCount - 1);

            if (dateGap.Length >= preferFromDate.Length)
                return -1;
            
            for (int i = 0; gapAvg > dateGap[i] && i< dateGap.Length - 1; i++)
            {
                int tmp = preferFromDate[i] - preferFromDate[i + 1];
                tmp /= (dateGap[i] - dateGap[i + 1]);
                tmp *= dateGap[i + 1] - (int)gapAvg;
                tmp = preferFromDate[i] - tmp;

                preference = Math.Max(tmp, 0);
            }
            
            /*
            gapAvg = (int)(Math.Max(gapAvg, 1));

            preference = (int)Math.Max(101-gapAvg , 0.0);
            preference = Math.Min(preference, 100);
            */
            return preference;
        }

        //public static SerPre


        
        private static int GetSimilarity(List<CusCraRelation> a_list, List<CusCraRelation> b_list)
        {
            if(a_list.Count == 0 || b_list.Count == 0)
            {
                return 50;
            }
            int similarity = 0;
            //평균 구하기
            int aAvg = 0;
            int bAvg = 0;
            for (int i = 0; i < a_list.Count; i++)
            {
                aAvg += a_list[i].Preference;
                bAvg += b_list[i].Preference;
            }
            aAvg /= a_list.Count;
            bAvg /= a_list.Count;

            //유사도 구하기
            int tmp1 = 0;
            int tmp2 = 0;
            int tmp3 = 0;
            for (int i = 0; i < a_list.Count; i++)
            {
                tmp1 += ((a_list[i].Preference - aAvg) * (b_list[i].Preference - bAvg));
                tmp2 += (a_list[i].Preference - aAvg) * (a_list[i].Preference - aAvg);
                tmp3 += (b_list[i].Preference - bAvg) * (b_list[i].Preference - bAvg);
            }

            double sim = tmp1 / (Math.Pow(tmp2, 0.5) * Math.Pow(tmp3, 0.5));

            sim += 1;
            //최대 100으로 맞추기
            similarity = (int)(sim * 50);

            return similarity;
        }

        public static int GetSimilarity(Cracker a, Cracker b)
        {
            int similarity = 0;
            var sql = "SELECT * " +
                "FROM CusCraRelation " +
                "WHERE CrackerUN = {0} " +
                "Order By CustomerUN ";

            DBConnecter connecter = new DBConnecter();

            var a_list = connecter.GetList<CusCraRelation>(String.Format(sql, a.UniqueNumber));
            var b_list = connecter.GetList<CusCraRelation>(String.Format(sql, b.UniqueNumber));

            if (a_list.Count != b_list.Count)
            {
                Console.WriteLine("Error : Cracker {0}과 Cracker {1} 의 CusCraRelation의 개수가 다릅니다.", a.Name, b.Name);
                return -1;
            }

            similarity = GetSimilarity(a_list, b_list);



            connecter.Close();
            return similarity;
        }

        public static int GetSimilarity(Customer a, Customer b)
        {
            int similarity = 0;
            var sql = "SELECT * " +
                "FROM CusCraRelation " +
                "WHERE CustomerUN = {0} " +
                "Order By CrackerUN ";

            DBConnecter connecter = new DBConnecter();

            var a_list = connecter.GetList<CusCraRelation>(String.Format(sql, a.UniqueNumber));
            var b_list = connecter.GetList<CusCraRelation>(String.Format(sql, b.UniqueNumber));

            if (a_list.Count != b_list.Count)
            {
                Console.WriteLine("Error : Customer {0}과 Customer {1} 의 CusCraRelation의 개수가 다릅니다.", a.Name, b.Name);
                return -1;
            }

            similarity = GetSimilarity(a_list, b_list);



            connecter.Close();
            return similarity;
        }
    }
}
