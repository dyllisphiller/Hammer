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
        }
    }
}
