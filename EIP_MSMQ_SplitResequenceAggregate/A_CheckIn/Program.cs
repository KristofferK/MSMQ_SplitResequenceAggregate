using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_CheckIn
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static string GetContentOfXmlFile()
        {
            var projectFolder = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var path = projectFolder + "\\xml-file\\FlightDetailsInfoResponse.xml";
            return File.ReadAllText(path);
        }
    }
}
