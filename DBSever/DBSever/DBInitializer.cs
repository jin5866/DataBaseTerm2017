using DBServer.DBdata;
using Server;
using Server.DBdata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSever
{
    static class DBInitializer
    {
        public static void Initializer()
        {
            var cracra = "select * from CraCraRelation where CrackerUN1 = {0} and CrackerUN2 = {1}";
            var cuscus = "select * from CusCusRelation where CustomerUN1 = {0} and CustomerUN2 = {1}";
            var cuscra = "select * from CusCraRelation where CustomerUN = {0} and CrackerUN = {1}";

            DBConnecter connecter = new DBConnecter();

            List<Cracker> craList = connecter.GetList<Cracker>();
            List<Customer> cusList = connecter.GetList<Customer>();

            foreach(Cracker cracker1 in craList)
            {
                foreach(Cracker cracker2 in craList)
                {
                    if(cracker1.UniqueNumber < cracker2.UniqueNumber)
                    {
                        var a = connecter.GetList<CraCraRelation>(String.Format(cracra, cracker1.UniqueNumber, cracker2.UniqueNumber));
                        if (a.Count() == 0)
                        {
                            CraCraRelation tmp = new CraCraRelation();

                            tmp.CrackerUN1 = cracker1.UniqueNumber;
                            tmp.CrackerUN2 = cracker2.UniqueNumber;
                            tmp.Similarity = 50;

                            connecter.Insert(tmp);
                        }
                    }
                }
            }



            foreach(Cracker cracker in craList)
            {
                foreach(Customer customer in cusList)
                {
                    if(connecter.GetList<CusCraRelation>(String.Format(cuscra, customer.UniqueNumber, cracker.UniqueNumber)).Count() == 0)
                    {
                        CusCraRelation tmp = new CusCraRelation();

                        tmp.CrackerUN = cracker.UniqueNumber;
                        tmp.CustomerUN = customer.UniqueNumber;
                        tmp.EatingCount = 0;
                        tmp.Preference = 50;

                        connecter.Insert(tmp);
                    }

                }
            }

            foreach (Customer customer1 in cusList)
            {
                foreach (Customer custoemr2 in cusList)
                {
                    if (customer1.UniqueNumber < custoemr2.UniqueNumber)
                    {
                        var a = connecter.GetList<CusCusRelation>();
                        a =connecter.GetList<CusCusRelation>(String.Format(cuscus, customer1.UniqueNumber, custoemr2.UniqueNumber));
                        if (a.Count() == 0)
                        {
                            CusCusRelation tmp = new CusCusRelation();

                            tmp.CustomerUN1 = customer1.UniqueNumber;
                            tmp.CustomerUN2 = custoemr2.UniqueNumber;
                            tmp.Similarity = 50;

                            connecter.Insert(tmp);
                        }
                    }
                }

            }

        }
    }
}
