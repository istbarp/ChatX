using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatXService;
using System.ServiceModel;

namespace ChatXServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ChatXService.ChatXService), new Uri("http://10.28.51.86:8984/ChatXService/")))
            {
                //host.Open();
                Console.WriteLine("Host is open");

                Console.WriteLine("Press ENTER to close host");
                Console.ReadLine();
            }
        }
    }
}
