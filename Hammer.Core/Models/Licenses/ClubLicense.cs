using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hammer.Core.Models
{
    public class ClubLicense : BaseLicense, IInTrust, INotifyPropertyChanged
    {
        private Trustee trustee;

        public Trustee Trustee
        {
            get => trustee;
            set
            {
                trustee = value;
                RaisePropertyChanged(nameof(Trustee));
            }
        }
    }
}
