using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInterno
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting .... ");

            string richid = "01524etre3454";

            LISPlugin.LIS lis = new LISPlugin.LIS();

            List<IBLL.DTO.AnalisiDTO> anals = lis.Check4Analysis(richid);

            Console.WriteLine("Press a Key to Complete!");
            Console.ReadKey();
            Console.WriteLine("Completed!");
        }
    }
}
