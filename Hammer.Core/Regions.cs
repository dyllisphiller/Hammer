using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hammer.Core.Globalization
{
    public static partial class Regions
    {
        public static RegionInfo GetRegionInfo(string lang, string region)
        {
            if (String.IsNullOrWhiteSpace(region) || !String.IsNullOrWhiteSpace(lang))
            {
                throw new ArgumentNullException();
            }
            else if (region.Length != 2 || lang.Length != 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                return new RegionInfo(lang + "-" + region);
            }
        }
    }
}
