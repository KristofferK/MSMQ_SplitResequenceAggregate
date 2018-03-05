using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class PassengerWithLuggage
    {
        public Passenger Passenger { get; set; }
        public Luggage[] Luggage { get; set; }

        public bool PassengerReady
        {
            get
            {
                return Passenger != null;
            }
        }

        public int LuggageReady
        {
            get
            {
                return Luggage.Where(e => e != null).Count();
            }
        }

        public bool IsReady
        {
            get
            {
                return PassengerReady && LuggageReady == Luggage.Length;
            }
        }

        public int ReadyPercentage
        {
            get
            {
                var passengerInt = PassengerReady ? 1 : 0;
                var readyPercentage = 100.0 / (Luggage.Length + 1) * (LuggageReady + passengerInt);
                return Convert.ToInt32(Math.Round(readyPercentage, 0));
            }
        }

        public PassengerWithLuggage(int luggageSize)
        {
            Luggage = new Luggage[luggageSize];
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"Passenger: {Passenger.ToString()}\n");
            for (var i = 0; i < Luggage.Length; i++)
            {
                sb.AppendLine($"Luggage #{i + 1}: {Luggage[i]}");
            }
            sb.AppendLine("Ready percentage: " + ReadyPercentage);
            sb.AppendLine("Is ready to be sent: " + IsReady);
            return sb.ToString();
        }
    }
}
