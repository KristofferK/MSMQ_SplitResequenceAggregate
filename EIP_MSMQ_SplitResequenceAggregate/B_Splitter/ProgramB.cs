using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace B_Splitter
{
    class ProgramB
    {
        private static MessageQueue inputChannel;
        private static MessageQueue outputLuggageChannel;
        private static MessageQueue outputPassangerChannel;
        private static Random random = new Random();

        static void Main(string[] args)
        {
            Console.Title = "System B (Splitter)";
            Console.WriteLine("System B (Splitter). Waiting for message from System A.");

            inputChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.AToBChannel);
            outputLuggageChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.BToCChannel);
            outputPassangerChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.BToDChannel);

            ReceiveInputFromSystemA();

            Console.ReadLine();
        }

        private static void ReceiveInputFromSystemA()
        {
            inputChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            inputChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemA);
            inputChannel.BeginReceive();
        }

        private static void HandleInputFromSystemA(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (string)message.Body;

            Console.WriteLine("Received " + message.Label);
            Console.WriteLine(body + "\n\n");

            var xml = new XmlDocument();
            xml.LoadXml(body);
            var passengerXml = xml.SelectSingleNode("/FlightDetailsInfoResponse/Passenger");
            var luggageXml = xml.SelectNodes("/FlightDetailsInfoResponse/Luggage");

            var passengerObj = Passenger.Create(passengerXml);
            Console.WriteLine("Sending passenger: " + passengerObj);
            outputPassangerChannel.Send(passengerObj);

            var packageId = Guid.NewGuid();
            var numbersInRandomOrder = Enumerable.Range(0, luggageXml.Count).OrderBy(e => Guid.NewGuid()).ToArray();
            for (var i = 0; i < luggageXml.Count; i++)
            {
                var packageNo = numbersInRandomOrder[i];
                Console.WriteLine($"Sending luggage {packageNo + 1} of {luggageXml.Count}.");
                outputLuggageChannel.Send(new PackageWrapper<Luggage>()
                {
                    PackageId = packageId,
                    PackageNumber = packageNo + 1,
                    PackageCount = luggageXml.Count,
                    Body = Luggage.Create(luggageXml.Item(packageNo))
                });
            }

            messageQueue.BeginReceive();
        }
    }
}
