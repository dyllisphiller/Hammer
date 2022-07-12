using System.ComponentModel;

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

        public ClubLicense()
        {
            LicenseeType = LicenseeTypes.Club;
        }
    }
}
