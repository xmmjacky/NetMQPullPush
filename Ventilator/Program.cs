using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventilator
{
    /// <summary>
    /// Pipeline 管道模式
    /// </summary>
  public  class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== VENTILATOR ======");

            Console.WriteLine("Press enter when worker are ready");
            Console.ReadLine();

            //the first message it "0" and signals start of batch  
            //see the Sink.csproj Program.cs file for where this is used  
            Console.WriteLine("Sending start of batch to Sink");

            var ventilator = new Ventilator();
            ventilator.Run();

            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();
        }
    }
}
