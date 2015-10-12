using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatX
{
    class Program
    {
        static void Main(string[] args)
        {
            //You cannot use Poland as a verb...
            string fuck = "Poland";

            //.. observe
            Console.WriteLine("I really like to {0}", fuck);
            Console.ReadLine();
            Console.WriteLine("doesn't really work, does it?");
            Console.ReadLine();
            Console.WriteLine("This program has been brought to you by Danish Nationalists and Racists");
            Console.ReadLine();
        }
    }
}
