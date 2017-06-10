using DBServer;
using DBServer.DBdata;
using Server;
using Server.DBdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBSever
{
    static class DataDealer
    {
        private const int SLEEP_TIME = 3600000;
        /// <summary>
        /// CraCraRel을 업데이트
        /// UPDATE CraCraRelation 
        /// SET Similarity = {0} 
        /// WHERE CrackerUN1 = {1} AND CrackerUN2 = {2}
        /// </summary>
        static string UpdateCraCraRel = "UPDATE CraCraRelation " +
            "SET Similarity = {0} " +
            "WHERE CrackerUN1 = {1} " +
            "AND CrackerUN2 = {2} ";

        static string UpdateCusCusRel = "UPDATE CusCusRelation " +
            "SET Similarity = {0} " +
            "WHERE CustomerUN1 = {1} " +
            "AND CustomerUN2 = {2} ";
        static string UpdateCusCraRel = "UPDATE CusCraRelation " +
            "SET preference = {0}, " +
            "EatingTimeAvg = {1} " +
            "WHERE CustomerUN = {2} " +
            "AND CrackerUN = {3}";

        static DBConnecter connecter = new DBConnecter();

        public static void ServerStart()
        {
            Thread dataDealer = new Thread(DataDealProcess);
            dataDealer.Start();

            Console.WriteLine("---------------------");
            Console.WriteLine("데이터 처리 서버 시작");
            Console.WriteLine("---------------------");
        }


        static void DataDealProcess()
        {
            while(true)
            {
                SetCusCraRelation();
                SetCraCraSimilarity();
                SetCusCusSimilarity();
                Thread.Sleep(SLEEP_TIME);
            }
        }

        static void SetCusCraRelation()
        {
            var list = connecter.GetList<CusCraRelation>();

            foreach(var i in list)
            {
                i.Preference = Core.GetPreference(i);
                if(i.EatingCount != 0)
                {
                    i.EatingTimeAvg = (DateTime.Now - i.FirstEat).Days / (i.EatingCount);
                }
                else
                {
                    i.EatingTimeAvg = 0;
                }


                connecter.SendSQL(String.Format(UpdateCusCraRel, i.Preference, i.EatingTimeAvg, i.CustomerUN, i.CrackerUN));
            }

        }
        static void SetCraCraSimilarity()
        {
            var list = connecter.GetList<Cracker>();

            foreach(var a in list)
            {
                foreach(var b in list)
                {
                    if (a.UniqueNumber < b.UniqueNumber) 
                    {
                        int sim = Core.GetSimilarity(a, b);

                        connecter.SendSQL(String.Format(UpdateCraCraRel, sim, a.UniqueNumber, b.UniqueNumber));
                    }
                }
            }
        }

        static void SetCusCusSimilarity()
        {
            var list = connecter.GetList<Customer>();

            foreach (var a in list)
            {
                foreach (var b in list)
                {
                    if (a.UniqueNumber < b.UniqueNumber)
                    {
                        int sim = Core.GetSimilarity(a, b);

                        connecter.SendSQL(String.Format(UpdateCusCusRel, sim, a.UniqueNumber, b.UniqueNumber));
                    }
                }
            }
        }
    }
}
