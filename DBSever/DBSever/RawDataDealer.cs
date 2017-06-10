using DBServer.DBdata;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBServer
{
    static class RawDataDealer
    {

        private const int SLEEP_TIME = 10000;

        static public void StartServer()
        {

            Thread queueCheck = new Thread(QueueChecker);

            queueCheck.Start();

            Console.WriteLine("---------------------");
            Console.WriteLine("Queue check 서버 시작 완료");
            Console.WriteLine("---------------------");

            Thread rawDataProcess = new Thread(RawDataProcesser);

            rawDataProcess.Start();

            Console.WriteLine("---------------------");
            Console.WriteLine("Data 처리 서버 시작 완료");
            Console.WriteLine("---------------------");
        }

        static void QueueChecker()
        {
            DBConnecter connecter = new DBConnecter();

            while (true)
            {
                lock (RawDataListener.lisQueue)
                {
                    foreach (byte[] item in RawDataListener.lisQueue)
                    {
                        Console.WriteLine(Encoding.Default.GetString(item));

                        RawData tmp = new RawData(item);

                        //db에 넣기
                        connecter.InsertRawData(tmp);
                        
                        
                    }

                    RawDataListener.lisQueue.Clear();
                }

                Thread.Sleep(1000);

            }
        }

        static void RawDataProcesser()
        {
            DBConnecter connecter = new DBConnecter();

            // 가장 오래된 다먹음 정보를 가져온다.
            var GetOldistData = "SELECT * "
                + "FROM RawData "
                + "WHERE EatTime = "
                + "(select min(EatTime) from RawData where DataType = 2);";

            // 찾은 정보에서 다먹었을때 먹기까지 정보를 가져온다.
            var FindSamePerson = "SELECT * "
                + "FROM RawData "
                + "WHERE CrackerUN = {0} AND "
                + "CustomerUN = {1} AND "
                + "EatTime <= (select min(EatTime) from RawData where CrackerUN = {0} AND CustomerUN = {1} AND DataType = 2) "
                + "ORDER BY EatTime";

            // 찾은 정보에서 다 먹었을때 까지의 정보를 지운다. {2} 는 최대시간.
            var deleteSame = "DELETE " +
                "FROM RawData " +
                "WHERE CrackerUN = {0} AND "
                + "CustomerUN = {1} AND "
                + "EatTime <= {2} " +
                "";
                

            while (true)
            {
                
                List<RawData> OldistRawData = connecter.GetList<RawData>(GetOldistData);
                if (OldistRawData != null && OldistRawData.Count != 0)
                {
                    foreach(var item in OldistRawData)
                    {
                        // 한쌍의 먹는 데이터
                        List<RawData> EatingDataSet = connecter.GetList<RawData>(String.Format(FindSamePerson, item.CrackerUN, item.CustomerUN));

                        int maxTime = 0;
                        //들어온 데이터로 처리할것
                        DealEatingData(EatingDataSet, ref maxTime , connecter);
                        
                        lock(connecter)
                        {
                            connecter.SendSQL(String.Format(deleteSame, item.CrackerUN, item.CustomerUN, maxTime));

                        }
                    }
                    
                }
                else
                {
                    // 들어온 데이터가 없을떈 10초 기다린다.
                    Thread.Sleep(SLEEP_TIME);
                }
            }
        }



        static void DealEatingData(List<RawData> EatingDataSet,ref int maxTime , DBConnecter connecter)
        {
            //
            var GetCusCraRelation = "Select * " +
                "From CusCraRelation " +
                "Where CustomerUN = {0} AND " +
                "CrackerUN = {1} ";

            //변경된 값을 저장.
            var Update = "UPDATE CusCraRelation " +
                "Set EatingCount = {0}, " +
                "EatingTimeAvg = {1} ," +
                "EatingTermAvg = {2}, " +
                "Preference = {3}" +
                "Where CustomerUN = {4} AND " +
                "CrackerUN = {5}";

            //처음 먹은 날자를 세팅함
            var SetDate = "UPDATE CusCraRelation " +
                "Set FirstEat = \"{0}\" " +
                "Where CustomerUN = {1} AND " +
                "CrackerUN = {2}";

            int minTime = EatingDataSet.First().EatTime;

            foreach (var EatingData in EatingDataSet)
            {
                if (EatingData.EatTime > maxTime)
                {
                    maxTime = EatingData.EatTime;
                }
                if(minTime > EatingData.EatTime)
                {
                    minTime = EatingData.EatTime;
                }
            }

            int term = RawData.SubEatTime(maxTime, minTime);

            int termAvg = RawData.EatTimeToSecond(term) / (EatingDataSet.Count - 1);

            var cusCraRel = connecter.GetList<CusCraRelation>(String.Format(GetCusCraRelation, EatingDataSet[0].CustomerUN, EatingDataSet[0].CrackerUN));

            // 먹은 횟수와 먹는 평균 속도 갱신
            if(cusCraRel[0].EatingCount == 0 || cusCraRel[0].EatingTermAvg == 0)
            {
                cusCraRel[0].EatingCount = 1;
                cusCraRel[0].EatingTermAvg = termAvg;
                connecter.SendSQL(String.Format(SetDate, RawData.timeToDateTime(minTime).ToString("yyyy-MM-dd hh:mm:ss"),
                    cusCraRel[0].CustomerUN, cusCraRel[0].CrackerUN));
            }
            else
            {
                cusCraRel[0].EatingTermAvg = 
                    (cusCraRel[0].EatingTermAvg * cusCraRel[0].EatingCount + termAvg) / (cusCraRel[0].EatingCount + 1);
                cusCraRel[0].EatingCount++;
            }

            cusCraRel[0].Preference = Core.GetPreference(cusCraRel[0]);

            //갱신된 데이터를 저장
            connecter.SendSQL(String.Format(Update, cusCraRel[0].EatingCount, cusCraRel[0].EatingTimeAvg,
                cusCraRel[0].EatingTermAvg, cusCraRel[0].Preference, cusCraRel[0].CustomerUN, cusCraRel[0].CrackerUN));

        }
    }
}
