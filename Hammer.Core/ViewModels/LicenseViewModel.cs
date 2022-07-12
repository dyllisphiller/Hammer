using Hammer.Core.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hammer.Core.ViewModels
{
    public class LicenseViewModel : INotifyPropertyChanged
    {
        private BaseLicense license;

        public BaseLicense License
        {
            get => license;
            set
            {
                license = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Overline));
            }
        }

        public bool IsClubLicense => license is ClubLicense;
        public bool IsPersonalLicense => license is PersonalLicense;

        public ClubLicense AsClubLicense => license as ClubLicense;
        public PersonalLicense AsPersonalLicense => license as PersonalLicense;

        public bool HasAddressAttn => !string.IsNullOrWhiteSpace(License.AddressAttn);

        public string Overline
        {
            get
            {
                string overline = string.IsNullOrWhiteSpace(License.Callsign.Sign) ? "" : License.Callsign.Sign;
                const string SEPARATOR = " • ";

                //if (IsPersonalLicense) overline += string.IsNullOrWhiteSpace(overline) ? $"{AsPersonalLicense.OperatorClass}" : $" ({AsPersonalLicense.OperatorClass})";
                //else if (IsClubLicense) overline += string.IsNullOrWhiteSpace(overline) ? $"{AsClubLicense.Trustee.Callsign}" : $"{SEPARATOR}{AsClubLicense.Trustee.Callsign}";

                return overline;
            }
        }

        public LicenseViewModel()
        {
            License = new TestLicense();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
