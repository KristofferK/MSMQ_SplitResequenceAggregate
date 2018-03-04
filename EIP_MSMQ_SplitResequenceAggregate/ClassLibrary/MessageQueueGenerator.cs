using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class MessageQueueGenerator
    {
        public static string AirlineToAirportRequestChannel { get; private set; } = @".\Private$\AirlineToAirportChannel";
        public static string AirportToAirlineReplyChannel { get; private set; } = @".\Private$\AirportToAirlineReplyChannel";

        public static MessageQueue GenerateMessageQueue(string messageQueueName)
        {
            if (!MessageQueue.Exists(messageQueueName))
            {
                MessageQueue.Create(messageQueueName);
            }
            return new MessageQueue(messageQueueName)
            {
                Label = "I'm located at " + messageQueueName
            };
        }
    }
}
