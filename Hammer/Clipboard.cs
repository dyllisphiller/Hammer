using Hammer.Core.Helpers;
using Windows.ApplicationModel.DataTransfer;

namespace Hammer
{
    class Clipboard : IClipboard
    {
        private readonly DataPackage dataPackage = new DataPackage();
        public void SetContent(string text)
        {
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(text);
        }
    }
}
