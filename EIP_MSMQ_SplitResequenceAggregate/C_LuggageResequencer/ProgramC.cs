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
        static void Main(string[] args)
        {
            Console.Title = "System C (Resequencer)";
            Console.WriteLine("System C (Resequencer). Waiting for luggage messages from System B.");

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
            Console.WriteLine("k");
            MessageQueue messageQueue = (MessageQueue)source;
            var message = messageQueue.EndReceive(asyncResult.AsyncResult);
            var body = (PackageWrapper<Luggage>)message.Body;

            Console.WriteLine($"Received {body}\n");

            messageQueue.BeginReceive();
        }
    }
}
