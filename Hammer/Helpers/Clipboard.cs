using Hammer.Core;
using Hammer.Core.Helpers;
using Windows.ApplicationModel.DataTransfer;

namespace Hammer.Helpers
{
    public class Clipboard : IClipboard
    {
        private readonly DataPackage dataPackage = new DataPackage();
        public void Copy(string text)
        {
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(text);
        }
    }
}
