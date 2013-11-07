using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Licensing
{
    public class License
    {
        public string LicenseType { get; set; }
        public DateTime ExpirationDate { get; internal set; }
    }
}
