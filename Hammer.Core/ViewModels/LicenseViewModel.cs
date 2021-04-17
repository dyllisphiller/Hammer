using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hammer.Core.Models
{
    public class LicenseViewModel : INotifyPropertyChanged
    {
        private static readonly BaseLicense defaultLicense;
        private BaseLicense license;

        public BaseLicense License
        {
            get => license ?? defaultLicense;
            set => license = value;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        static LicenseViewModel() => defaultLicense = Models.PersonalLicense.GetTestLicense();
    }
}
