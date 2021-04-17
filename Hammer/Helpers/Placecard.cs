using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Hammer.Helpers.Maps
{
    public static class Placecard
    {
        public static void ShowPlacecard(object sender, Geopoint geopoint, string displayName, string addressLine1, string addressLine2)
        {
            FrameworkElement targetElement = (FrameworkElement)sender;

            GeneralTransform generalTransform =
                    targetElement.TransformToVisual((FrameworkElement)targetElement.Parent);

            Rect rectangle = generalTransform.TransformBounds(new Rect(new Point
                (targetElement.Margin.Left, targetElement.Margin.Top), targetElement.RenderSize));

            try
            {
                PlaceInfoCreateOptions options = new PlaceInfoCreateOptions
                {
                    DisplayAddress = $"{addressLine1}, {addressLine2}",
                    DisplayName = displayName
                };

                PlaceInfo.Create(geopoint, options)
                         .Show(rectangle, Windows.UI.Popups.Placement.Below);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
