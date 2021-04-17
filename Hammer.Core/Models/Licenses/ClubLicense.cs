using System;
using System.Collections.Generic;
using System.Text;

namespace Hammer.Core.Models
{
    public class ClubLicense : BaseLicense, IInTrust
    {
        public Trustee Trustee { get; set; }
    }
}
