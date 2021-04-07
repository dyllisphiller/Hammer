using System;
using System.Linq;
using Hammer.Core.Models;

namespace Hammer.CLI
{
    class Program
    {
        enum Commands
        {
            None,
            Lookup
        }

        static void Main(string[] args)
        {
            Commands command;
            string callsign;

            Console.Write("Hammer CLI → ");

            if (args.Contains("-l") || args.Contains("--lookup"))
            {
                if (!string.IsNullOrWhiteSpace(args.ElementAtOrDefault(1)))
                {
                    callsign = args.ElementAtOrDefault(1);
                    command = Commands.Lookup;
                    Lookup(callsign);
                }
                else
                {
                    Console.WriteLine("Lookups require a callsign.");
                    return;
                }
            }
            else command = Commands.None;

            if (command == Commands.None)
            {
                Console.WriteLine("Help");
                Console.WriteLine("Hammer provides callsign lookups.");
                Console.WriteLine("To use Hammer, specify -l and a callsign.");
                Console.WriteLine();
                Console.WriteLine("EXAMPLES");
                Console.WriteLine("\tHammer.exe -l W1AW");
            }
        }

        static void Lookup(string callsign)
        {
            Console.WriteLine($"Callsign Lookup → {callsign}\n");

            License.GetLicenseByCallsign(callsign, out License license);

            license.Callsign = new Callsign(callsign);
            license.Name = "JOHNNY APPLESEED";
            license.FRN = "1234567";
            license.OperatorClass = "Extra";

            Console.WriteLine("=== LICENSE RECORD BEGINS ===");
            Console.WriteLine($"{license.Callsign} licensed to:");
            Console.WriteLine($"{license.Name}\n(FRN {license.FRN})");
            Console.WriteLine($"Amateur {license.OperatorClass}");
            Console.WriteLine(
                "ADDRESS:\n" +
                "\t1234 Anystreet Ave.\n" +
                "\tBrooklyn\tNY\t10001\n" +
                "\tUnited States\n\n" +
                "NO TRUSTEE\n" +
                "EXPIRES IN 2 YEARS"
                );
        }

        static void ExtendedLookup(string callsign)
        {
            Lookup(callsign);
        }
    }
}
