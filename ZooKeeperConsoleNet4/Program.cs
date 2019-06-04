using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooKeeperConsoleNet4
{
    public static class Program
    {


        static void Main(string[] args)
        {

            var w = new ZooKeeperClient();


            Console.ReadKey();

            publishEvent("nr of children in root: " + w.ChildrenCount("/") );

            foreach (string item in w.GetChildren("/")){publishEvent(item);}

            w.Close();


            Console.ReadKey();

            w.Connect();

            Console.ReadKey();

            w.Close();

            Console.ReadKey();

        }

        public  static void publishEvent(string s)
        {
            Console.WriteLine(s);
        }


    }



   

}
