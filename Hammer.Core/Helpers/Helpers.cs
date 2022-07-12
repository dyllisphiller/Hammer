using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hammer.Core.Helpers
{
    public static partial class Parsers
    {
        // This documentation is derived from the metadata for
        // Enum.TryParse<TEnum>(string, bool, out TEnum), which is the primary
        // internal mechanism of this method.
        /// <summary>
        /// Converts the string representation of the name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated object.
        /// The operation is case-insensitive and strips white-space characters
        /// from value before attempting the conversion.
        /// </summary>
        /// <typeparam name="TEnum">
        /// The enumeration type to which to convert value.
        /// </typeparam>
        /// <param name="value">
        /// The string representation of the enumeration name or underlying
        /// value to convert.
        /// </param>
        /// <returns>
        /// the equivalent enumerated object if the value parameter was
        /// converted successfully; otherwise, the enumerated object whose value
        /// is equal to the integer 0, which is usually TEnum.Unknown.
        /// </returns>
        public static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            value = value.Replace(" ", "");

            Enum.TryParse(value, true, out TEnum enumValue);
            return enumValue;
        }
    }

    public static partial class PropertyChangeNotifier
    {
        public static void RaisePropertyChanged(PropertyChangedEventHandler eventHandler, [CallerMemberName] string propertyName = null)
        {
            eventHandler?.Invoke(eventHandler, new PropertyChangedEventArgs(propertyName));
        }
    }
}
