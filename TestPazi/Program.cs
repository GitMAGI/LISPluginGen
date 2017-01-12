using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seminabit.Sanita.OrderEntry.LIS.BusinessLogicLayer;
using Seminabit.Sanita.OrderEntry.LIS.DataAccessLayer;
using Seminabit.Sanita.OrderEntry.LIS.IBLL.DTO;

namespace TestPazi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting .... ");

            LISDAL dal = new LISDAL();
            LISBLL bll = new LISBLL(dal);

            string cogn = "NJIBEBWE";
            string nom = "AYISELE";
            string sess = "F";
            DateTime dat = Convert.ToDateTime("1979-05-25 00:00:00");
            string cofi = "NJBYSL79E65Z311H";

            List<PazienteDTO> pazis = bll.GetPazienteBy5IdentityFields(cogn, nom, sess, dat, cofi);

            Console.WriteLine("Press a Key to Complete!");
            Console.ReadKey();
            Console.WriteLine("Completed!");
        }
    }
}
