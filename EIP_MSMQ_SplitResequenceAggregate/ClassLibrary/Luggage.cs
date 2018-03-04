using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClassLibrary
{
    public class Luggage
    {
        public string Id { get; set; }
        public int Identification { get; set; }
        public string Category { get; set; }
        public decimal Weight { get; set; }

        public override string ToString()
        {
            return $"{Id} #{Identification}. {Category} at {Weight} kg.";
        }

        public static Luggage Create(XmlNode node)
        {
            return new Luggage()
            {
                Id = node.SelectSingleNode("Id").InnerText,
                Identification = int.Parse(node.SelectSingleNode("Identification").InnerText),
                Category = node.SelectSingleNode("Category").InnerText,
                Weight = decimal.Parse(node.SelectSingleNode("Weight").InnerText)
            };
        }
    }
}
