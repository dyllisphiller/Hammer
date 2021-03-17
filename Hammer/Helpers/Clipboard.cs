using Hammer.Core.Helpers;
using Windows.ApplicationModel.DataTransfer;

namespace Hammer.Helpers
{
    public class Clipboard : IClipboard
    {
        public void Copy(string text)
        {
            DataPackage dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(text);
        }
    }
}
