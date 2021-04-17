using Hammer.Core.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hammer.Core.ViewModels
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

        static LicenseViewModel() => defaultLicense = PersonalLicense.GetTestLicense();

        public LicenseViewModel()
        {
            License.PropertyChanged += License_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void License_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BaseLicense _license = sender as BaseLicense;

        }
    }
}
