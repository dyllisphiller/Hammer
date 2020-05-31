using System.Collections.Generic;

namespace Hammer.Core.Contacts
{
    public enum ContactFieldType
    {
        /// <summary>
        /// The contact's email address. Supported on Windows Phone.
        /// </summary>
        Email = 0,

        /// <summary>
        /// The contact's phone number. Supported on Windows Phone.
        /// </summary>
        PhoneNumber = 1,

        /// <summary>
        /// The contact's location.
        /// </summary>
        Location = 2,

        // Summary:
        //     The contact's instant message user name.
        InstantMessage = 3,
        //
        // Summary:
        //     A custom value.
        Custom = 4,
        //
        // Summary:
        //     The contact's connected service account.
        ConnectedServiceAccount = 5,
        //
        // Summary:
        //     The contact's important dates.
        ImportantDate = 6,
        //
        // Summary:
        //     The contact's address. Supported on Windows Phone.
        Address = 7,
        //
        // Summary:
        //     The contact's significant other.
        SignificantOther = 8,
        //
        // Summary:
        //     The contact's notes.
        Notes = 9,
        //
        // Summary:
        //     The contact's Web site.
        Website = 10,
        //
        // Summary:
        //     The contact's job info.
        JobInfo = 11
    }
    public interface IContactField
    {
        string Name { get; }
        ContactFieldType Type { get; }
        string Value { get; }
    }

    internal interface IContact
    {
        IList<IContactField> Fields { get; }
        string Name { get; set; }
        string Callsign { get; set; }
    }
}
