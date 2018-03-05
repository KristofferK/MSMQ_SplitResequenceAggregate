using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace C_LuggageResequencer
{
    class ProgramC
    {
        private static MessageQueue inputChannel;
        private static MessageQueue outputChannel;
        private static Dictionary<string, PackageWrapper<Luggage>[]> luggageReceived;

        static void Main(string[] args)
        {
            Console.Title = "System C (Resequencer)";
            Console.WriteLine("System C (Resequencer). Waiting for luggage messages from System B.");

            luggageReceived = new Dictionary<string, PackageWrapper<Luggage>[]>();

            inputChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.BToCChannel);
            outputChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.CToDChannel);

            ReceiveInputFromSystemB();

            Console.ReadLine();
        }

        private static void ReceiveInputFromSystemB()
        {
            inputChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(PackageWrapper<Luggage>) });
            inputChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemB);
            inputChannel.BeginReceive();
        }

        private static void HandleInputFromSystemB(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (PackageWrapper<Luggage>)message.Body;

            Console.WriteLine($"Received {body}\n");

            var bodyGuid = body.PackageId.ToString();
            if (!luggageReceived.ContainsKey(bodyGuid))
            {
                luggageReceived[bodyGuid] = new PackageWrapper<Luggage>[body.PackageCount];
            }
            luggageReceived[bodyGuid][body.PackageNumber - 1] = body;

            if (luggageReceived[bodyGuid].Where(e => e != null).Count() == body.PackageCount)
            {
                Console.WriteLine("\nAll package for " + body.PackageId + " has been received!\n");
                var luggagesOrdered = luggageReceived[bodyGuid].OrderBy(e => e.PackageNumber);
                for (var i = 0; i < luggagesOrdered.Count(); i++)
                {
                    var luggage = luggagesOrdered.ElementAt(i);
                    var luggageMessage = new Message()
                    {
                        Label = $"Luggage {luggage.PackageNumber}/{luggage.PackageCount} from {luggage.PackageId}",
                        Body = luggage
                    };
                    Console.WriteLine("Sending " + luggageMessage.Body);
                    outputChannel.Send(luggageMessage);
                }
            }

            messageQueue.BeginReceive();
        }
    }
}
