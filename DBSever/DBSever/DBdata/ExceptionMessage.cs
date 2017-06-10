using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DBdata
{
    static class ExceptionMessage
    {
        static public void PrintSQLException(Exception e)
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("SQL Execption message");
            Console.WriteLine(e.Message);
            Console.WriteLine("---------------------");
        }

        static public void PrintListenFail()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Listen Fail");
            Console.WriteLine("---------------------");
        }

        static public void PrintListenFailOnListening()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Listen Fail On Listening");
            Console.WriteLine("---------------------");
        }
    }
}
