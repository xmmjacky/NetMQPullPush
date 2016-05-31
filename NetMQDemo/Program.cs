using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NetMQContext context = NetMQContext.Create())
            {
                Server(context);
            }

            Console.ReadKey();
        }
        static int Calc(int a, int b)
        {
            return a + b;
        }
        static void Server(NetMQContext context)
        {
            using (NetMQSocket serverSocket = context.CreateResponseSocket())
            {
                serverSocket.Bind("tcp://127.0.0.1:5555");
                byte[] buffer = serverSocket.ReceiveFrameBytes();
                Message msg = (Message)ByteConvertHelper.Bytes2Object(buffer);
                msg.Result = (Calc((int)msg.Params["a"], (int)msg.Params["b"]).ToString());
                Console.WriteLine("Result:" + msg.Result);
                serverSocket.SendFrame("Result:" + msg.Result);
            }
        }
    }
}
