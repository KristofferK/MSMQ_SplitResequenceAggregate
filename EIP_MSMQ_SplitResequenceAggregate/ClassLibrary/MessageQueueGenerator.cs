using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class MessageQueueGenerator
    {
        public static string AToBChannel { get; private set; } = @".\Private$\EIP_SRA_A_To_B_Channel";
        public static string BToCChannel { get; private set; } = @".\Private$\EIP_SRA_B_To_C_Channel";
        public static string BToDChannel { get; private set; } = @".\Private$\EIP_SRA_B_To_D_Channel";
        public static string CToDChannel { get; private set; } = @".\Private$\EIP_SRA_C_To_D_Channel";

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
