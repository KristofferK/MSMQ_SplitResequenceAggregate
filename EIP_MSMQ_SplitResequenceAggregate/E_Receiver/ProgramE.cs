using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace E_Receiver
{
    class ProgramE
    {
        private static MessageQueue inputChannel;

        static void Main(string[] args)
        {
            Console.Title = "System E (Receiver)";
            Console.WriteLine("System E (Receiver). Waiting for aggregated messages from System D.");

            inputChannel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.DToEChannel);

            ReceiveInputFromSystemD();

            Console.ReadLine();
        }

        private static void ReceiveInputFromSystemD()
        {
            inputChannel.Formatter = new XmlMessageFormatter(new Type[] { typeof(PassengerWithLuggage) });
            inputChannel.ReceiveCompleted += new ReceiveCompletedEventHandler(HandleInputFromSystemD);
            inputChannel.BeginReceive();
        }

        private static void HandleInputFromSystemD(object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue messageQueue = (MessageQueue)source;
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (PassengerWithLuggage)message.Body;

            Console.WriteLine($"Received passenger with luggage");
            Console.WriteLine(body);

            messageQueue.BeginReceive();
        }
    }
}
