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

        static void Main(string[] args)
        {
            Console.Title = "System D (Aggregator)";
            Console.WriteLine("System D (Aggregator). Waiting for passenger messages from System B or luggage messages from System C.");

            inputPassengerChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.BToDChannel);
            inputLuggageChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.CToDChannel);
            outputChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.DToEChannel);

            ReceiveInputFromSystemB();
            ReceiveInputFromSystemC();

            Console.ReadLine();
        }

        private static void ReceiveInputFromSystemB()
        {
            inputPassengerChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(Passenger) });
            inputPassengerChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemB);
            inputPassengerChannel.BeginReceive();
        }

        private static void ReceiveInputFromSystemC()
        {
            inputLuggageChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(Luggage) });
            inputLuggageChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemC);
            inputLuggageChannel.BeginReceive();
        }

        private static void HandleInputFromSystemB(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (Passenger)message.Body;

            Console.WriteLine($"Received passenger: {body}\n");

            messageQueue.BeginReceive();
        }

        private static void HandleInputFromSystemC(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = message.Body;

            Console.WriteLine($"Received luggage: {body}\n");

            messageQueue.BeginReceive();
        }
    }
}
