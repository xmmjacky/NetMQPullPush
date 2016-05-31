using Model;
using NetMQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public sealed class Worker
    {
        public void Run()
        {
            Task.Run(() =>
            {
            using (NetMQContext ctx = NetMQContext.Create())
            {
                //socket to receive messages on  
                using (var receiver = ctx.CreatePullSocket())
                {
                    receiver.Connect("tcp://localhost:5557");

                    //socket to send messages on  
                    using (var sender = ctx.CreatePushSocket())
                    {
                        sender.Connect("tcp://localhost:5558");

                        //process tasks forever  
                        while (true)
                        {
                            //workload from the vetilator is a simple delay  
                            //to simulate some work being done, see  
                            //Ventilator.csproj Proram.cs for the workload sent  
                            //In real life some more meaningful work would be done  

                            //string workload = receiver.ReceiveString();  

                            var receivedBytes = receiver.Receive();
                            using (var sm = new MemoryStream(receivedBytes))
                            {
                                //Protobuf.net 序列化在多线程方式下报错:  
                                /* 
                                  Timeout while inspecting metadata; this may indicate a deadlock.  
                                  This can often be avoided by preparing necessary serializers during application initialization,  
                                  rather than allowing multiple threads to perform the initial metadata inspection;  
                                  please also see the LockContended event 
                                 */
                                //var person = Serializer.Deserialize<Person>(sm);  

                                //采用二进制方式  
                                var binaryFormatter = new BinaryFormatter();
                                var person = binaryFormatter.Deserialize(sm) as Person;
                                Console.WriteLine("Person {Id:" + person.Id + ",Name:" + person.Name + ",BirthDay:" +
                                                  person.BirthDay + ",Address:{Line1:" + person.Address.Line1 +
                                                  ",Line2:" + person.Address.Line2 + "}}");
                                Console.WriteLine("Sending to Sink:" + person.Id);
                                sender.Send(person.Id + "");
                            }

                            //simulate some work being done  
                            //Thread.Sleep(int.Parse(workload));  
                        }
                        }
                    }
                }
            });
        }
    }
}
