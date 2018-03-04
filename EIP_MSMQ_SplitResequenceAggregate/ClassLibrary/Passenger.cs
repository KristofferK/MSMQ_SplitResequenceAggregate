using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClassLibrary
{
    public class Passenger
    {
        public string ReservationNumber;
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({ReservationNumber})";
        }

        public static Passenger Create(XmlNode node)
        {
            return new Passenger()
            {
                FirstName = node.SelectSingleNode("FirstName").InnerText,
                LastName = node.SelectSingleNode("LastName").InnerText,
                ReservationNumber = node.SelectSingleNode("ReservationNumber").InnerText
            };
        }
    }
}
