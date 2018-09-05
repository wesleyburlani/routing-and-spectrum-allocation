using System;
using System.Collections.Generic;

namespace RoutingAndSpectrumAllocation.RSA
{
    [Serializable]
    public class RSATableElement
    {
        public RSATableElement()
        {
            IsProtectionDemand = false;
            Values = new List<string>();
        }

        public bool IsProtectionDemand { get; set; }
        public List<string> Values { get; set; }
    }
}
