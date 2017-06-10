using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServer
{
    static class Setting
    {
        static public int MONTH_DATE { get; private set; }
        static public int BEFORE_MONTH_DATE { get; private set; }
        public static void SetMonthDate()
        {
            switch(DateTime.Now.Month)
            {
                case 1:
                    MONTH_DATE = 31;
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 2:
                    if(DateTime.Now.Year%4 == 0)
                    {
                        MONTH_DATE = 29;
                    }
                    else
                    {
                        MONTH_DATE = 28;
                    }
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 3:
                    MONTH_DATE = 31;
                    if (DateTime.Now.Year % 4 == 0)
                    {
                        BEFORE_MONTH_DATE = 29;
                    }
                    else
                    {
                        BEFORE_MONTH_DATE = 28;
                    }
                    break;
                case 4:
                    MONTH_DATE = 30;
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 5:
                    MONTH_DATE = 31;
                    BEFORE_MONTH_DATE = 30;
                    break;
                case 6:
                    MONTH_DATE = 30;
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 7:
                    MONTH_DATE = 31;
                    BEFORE_MONTH_DATE = 30;
                    break;
                case 8:
                    MONTH_DATE = 31;
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 9:
                    MONTH_DATE = 30;
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 10:
                    MONTH_DATE = 31;
                    BEFORE_MONTH_DATE = 30;
                    break;
                case 11:
                    MONTH_DATE = 30;
                    BEFORE_MONTH_DATE = 31;
                    break;
                case 12:
                    MONTH_DATE = 31;
                    BEFORE_MONTH_DATE = 30;
                    break;

                default:
                    MONTH_DATE = 30;
                    break;

            }
        }
    }
}
