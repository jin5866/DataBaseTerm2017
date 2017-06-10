using DBServer;
using Server.DBdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DBServer.DBdata;
using DBSever;

namespace Server
{
    class Program
    {
        static bool isServerOn = false;


        static void Main(string[] args)
        {
            MainMenu();
        }



        static void MainMenu()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("1. 서버시작");
                Console.WriteLine("2. 데이터 베이스 정리");
                Console.WriteLine("3. 랜덤 고객 추가");
                Console.WriteLine("4. 종료");

                int select = Console.Read() - '0';

                switch (select)
                {
                    case 1:
                        StartServer();
                        break;
                    case 2:
                        DBInitializer.Initializer();
                        break;
                    case 3:
                        RandomCustomer();
                        break;
                    case 4:
                        return;
                    default:
                        break;
                }
            }
        }
        static void StartServer()
        {
            if(!isServerOn)
            {
                Console.Clear();

                Setting.SetMonthDate();

                RawDataListener.StartServer();

                RawDataDealer.StartServer();

                DataDealer.ServerStart();

                isServerOn = true;
            }
            else
            {
                Console.WriteLine("이미 서버가 켜져있습니다.");
            }

            while(true)
            {
                if(Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    int tmp;
                    Console.WriteLine("정말 종료하시겠 습니까? Y/N");
                    tmp = Console.Read();
                    if ( tmp == 'Y' || tmp == 'y')
                    {
                        return;

                    }
                    else
                    {
                        //아무일 없음
                    }
                }
            }
        }

        static void RandomCustomer()
        {
            int tmp;
            Console.WriteLine("--------------");
            Console.WriteLine("몇명까지?");
            var num = Console.ReadLine();
            while (!Int32.TryParse(num, out tmp))
            {
                num = Console.ReadLine();
            }
            RandomData.AddRandomCustomer(tmp);
        }
    }
}
