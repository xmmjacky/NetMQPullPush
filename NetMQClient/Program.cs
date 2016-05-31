using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace NetMQClient
{
   public class Program
    {
        static void Main(string[] args)
        {
            using (NetMQContext context = NetMQContext.Create())
            {
                Client(context);
            }

            Console.ReadKey();
        }
        static void Client(NetMQContext context)
        {
            using (NetMQSocket clientSocket = context.CreateRequestSocket())
            {
                clientSocket.Connect("tcp://127.0.0.1:5555");

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("a", 1);
                dic.Add("b", 2);

                Message msg = new Message()
                {
                    Id = 0,
                    FunName = "Calc",
                    Params = dic,
                    Result = ""
                };

                byte[] buffer = ByteConvertHelper.Object2Bytes(msg);
                clientSocket.SendFrame(buffer);
                string answer = clientSocket.ReceiveFrameString();
                Console.WriteLine("Answer from server: {0}", answer);
            }
        }
    }
}
