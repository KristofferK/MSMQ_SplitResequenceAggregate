using ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace A_CheckIn
{
    class ProgramA
    {
        private static MessageQueue channel;
        private static bool keepSendingMessages = false;
        private static Random random = new Random();

        static void Main(string[] args)
        {
            Console.Title = "System A";
            Console.WriteLine("System A. Sending content of the XML file to System B.");

            channel = MessageQueueGenerator.GenerateMessageQueue(MessageQueueGenerator.AToBChannel);

            new Thread(() =>
            {
                SendMessageToSystemB();
                while (keepSendingMessages)
                {
                    var sleepDuration = random.Next(20, 50) * 100;
                    Console.WriteLine($"Waiting {sleepDuration} ms");
                    Thread.Sleep(sleepDuration);
                    SendMessageToSystemB();
                }
            }).Start();

            Console.ReadLine();
        }

        private static void SendMessageToSystemB()
        {
            var message = new Message()
            {
                Label = "Flight details info response",
                Body = GetContentOfXmlFile()
            };
            channel.Send(message);
            Console.WriteLine("Sent " + message.Label);
            Console.WriteLine(message.Body);
        }

        private static string GetContentOfXmlFile()
        {
            var projectFolder = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var path = projectFolder + "\\xml-file\\FlightDetailsInfoResponse.xml";
            return File.ReadAllText(path);
        }
    }
}
