using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace D_Receiver
{
    class ProgramD
    {
        private static MessageQueue inputPassengerChannel;
        private static MessageQueue inputLuggageChannel;
        private static MessageQueue outputChannel;
        private static Dictionary<Guid, PassengerWithLuggage> inputPassengerWithLuggage;

        static void Main(string[] args)
        {
            Console.Title = "System D (Aggregator)";
            Console.WriteLine("System D (Aggregator). Waiting for passenger messages from System B or luggage messages from System C.");

            inputPassengerWithLuggage = new Dictionary<Guid, PassengerWithLuggage>();

            inputPassengerChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.BToDChannel);
            inputLuggageChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.CToDChannel);
            outputChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.DToEChannel);

            ReceiveInputFromSystemB();
            ReceiveInputFromSystemC();

            Console.ReadLine();
        }

        private static void ReceiveInputFromSystemB()
        {
            inputPassengerChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(PackageWrapper<Passenger>) });
            inputPassengerChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemB);
            inputPassengerChannel.BeginReceive();
        }

        private static void ReceiveInputFromSystemC()
        {
            inputLuggageChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(PackageWrapper<Luggage>) });
            inputLuggageChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemC);
            inputLuggageChannel.BeginReceive();
        }

        private static void HandleInputFromSystemB(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            messageQueue.BeginReceive();
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (PackageWrapper<Passenger>)message.Body;

            Console.WriteLine($"Received passenger: {body}\n");
            AddToDictionaryIfNotExists(body.PackageId, body.PackageCount);
            inputPassengerWithLuggage[body.PackageId].Passenger = body.Body;
            SendIfReady(body.PackageId);

        }

        private static void HandleInputFromSystemC(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            messageQueue.BeginReceive();
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (PackageWrapper<Luggage>)message.Body;

            Console.WriteLine($"Received luggage: {body}\n");
            AddToDictionaryIfNotExists(body.PackageId, body.PackageCount);
            inputPassengerWithLuggage[body.PackageId].Luggage[body.PackageNumber - 1] = body.Body;
            SendIfReady(body.PackageId);

        }

        private static void AddToDictionaryIfNotExists(Guid packageId, int packageCount)
        {
            if (!inputPassengerWithLuggage.ContainsKey(packageId))
            {
                inputPassengerWithLuggage[packageId] = new PassengerWithLuggage(packageCount);
            }
        }

        private static void SendIfReady(Guid packageId)
        {
            var passengerWithLuggage = inputPassengerWithLuggage[packageId];
            Console.WriteLine(passengerWithLuggage);
            if (passengerWithLuggage.IsReady)
            {
                Console.WriteLine("Sending passenger with luggage to System E");
                outputChannel.Send(new Message()
                {
                    Label = "Passenger and luggage " + packageId.ToString(),
                    Body = passengerWithLuggage
                });
                inputPassengerWithLuggage.Remove(packageId);
            }
            Console.WriteLine("\n\n--------------\n");
        }
    }
}
