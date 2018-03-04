using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class PackageWrapper<T>
    {
        public Guid PackageId { get; set; }
        public int PackageNumber { get; set; }
        public int PackageCount { get; set; }
        public T Body { get; set; }

        public override string ToString()
        {
            return $"{PackageNumber} of {PackageCount} from {PackageId} contains:\n{Body}";
        }
    }
}
