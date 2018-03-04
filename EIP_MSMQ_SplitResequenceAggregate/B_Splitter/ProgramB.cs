using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace B_Splitter
{
    class Program
    {
        private static MessageQueue inputChannel;
        private static MessageQueue outputLuggageChannel;
        private static MessageQueue outputPassangerChannel;

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

            Console.WriteLine("Received " + message.Label);
            Console.WriteLine(message.Body + "\n\n");

            outputLuggageChannel.Send(null);
            outputLuggageChannel.Send(null);
            messageQueue.BeginReceive();
        }
    }
}
