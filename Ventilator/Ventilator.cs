using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ventilator
{
   public sealed class Ventilator
    {
        public void Run()
        {
            Task.Run(() =>
            {
                using (var ctx = NetMQContext.Create())
                using (var sender = ctx.CreatePushSocket())
                using (var sink = ctx.CreatePushSocket())
                {
                    sender.Bind("tcp://*:5557");
                    sink.Connect("tcp://localhost:5558");
                    sink.Send("0");

                    Console.WriteLine("Sending tasks to workers");
                    //RuntimeTypeModel.Default.MetadataTimeoutMilliseconds = 300000;

                    //send 100 tasks (workload for tasks, is just some random sleep time that  
                    //the workers can perform, in real life each work would do more than sleep  
                    for (int taskNumber = 0; taskNumber < 100000; taskNumber++)
                    {
                        Console.WriteLine("Workload : {0}", taskNumber);
                        var person = new Person
                        {
                            Id = taskNumber,
                            Name = "First",
                            BirthDay = DateTime.Parse("1981-11-15"),
                            Address = new Address { Line1 = "Line1", Line2 = "Line2" }
                        };
                        using (var sm = new MemoryStream())
                        {
                            //Serializer.PrepareSerializer<Person>();  
                            //Serializer.Serialize(sm, person);  
                            //sender.Send(sm.ToArray());  

                            var binaryFormatter = new BinaryFormatter();
                            binaryFormatter.Serialize(sm, person);
                            sender.Send(sm.ToArray());
                        }
                    }
                }
            });
        }
    }
}
