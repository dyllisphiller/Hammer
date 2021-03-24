using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hammer.Core.Models.Converters.US
{
	public enum USStatus
    {
		Valid,
		Invalid,
		Updating,
    }

	public enum USLicenseType
    {
		Person,
		Club,
		Recreation,
		Races,
		Military,
    }

	public enum USOperatorClass
    {
		Novice,
		Technician,
		TechnicianPlus,
		General,
		Advanced,
		Extra,
    }

    public class USLicense
    {
		public USStatus Status { get; set; }
		public USLicenseType Type { get; set; }
		public USLicenseData Current { get; set; }
		public USLicenseData Previous { get; set; }
		public USTrusteeData Trustee { get; set; }
		public string Name { get; set; }
		public USAddress Address { get; set; }
		public USLocation Location { get; set; }
		public USOtherInfo OtherInfo { get; set; }
    }

	public static partial class ConverterExtensions
    {
		public static async Task<License> ToLicenseAsync(this USLicense usLicense)
        {
			throw new NotImplementedException();
        }
    }

	public struct USLicenseData
    {
		public Callsign Callsign;
		public USOperatorClass OperClass;
    }

	public struct USTrusteeData
    {
		public Callsign Callsign;
		public string Name;
    }

	public struct USAddress
    {
		public string Line1;
		public string Line2;
		public string Attn;
	}

	public struct USLocation
    {
		public decimal Latitude;
		public decimal Longitude;
		public string GridSquare;
	}

	public struct USOtherInfo
    {
		public DateTimeOffset GrantDate;
		public DateTimeOffset ExpiryDate;
		public DateTimeOffset LastActionDate;
		public string FRN;
		public Uri UlsUri;
	}
}
