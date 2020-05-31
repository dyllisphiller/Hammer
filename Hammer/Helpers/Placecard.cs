using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Services.Maps;

namespace Hammer.Helpers.Cartography
{
    public static class Placecard
    {
        public static void ShowPlacecard(object sender, Geopoint geopoint, string callsign, string addressLine1, string addressLine2)
        {
            var _options = new PlaceInfoCreateOptions();
            _options.DisplayName = callsign;

            PlaceInfoCreateOptions options = new PlaceInfoCreateOptions
            {
                DisplayAddress = addressLine1 + ", " + addressLine2,
                DisplayName = callsign
            };

            PlaceInfo licensePlaceInfo = PlaceInfo.Create(geopoint, options);

            if (sender == null) { throw new ArgumentNullException(nameof(sender), "Hammer.Helpers.Cartography.Placecard.ShowPlacecard() parameter 'sender' must not be null."); }


            FrameworkElement targetElement = (FrameworkElement)sender;


            GeneralTransform generalTransform =
                    targetElement.TransformToVisual((FrameworkElement)targetElement.Parent);

            Rect rectangle = generalTransform.TransformBounds(new Rect(new Point
                (targetElement.Margin.Left, targetElement.Margin.Top), targetElement.RenderSize));

            try
            {
                licensePlaceInfo.Show(rectangle, Windows.UI.Popups.Placement.Below);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
