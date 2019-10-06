using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hammer.Core.Callsigns
{
    public static class Prefixes
    {
        private static IDictionary<string, string> RawPrefixes = new Dictionary<string, string>
        {
            // AD - Andorra
            { @"^C3", "ad" },

            // AE - United Arab Emirates
            { @"^A6", "ae" },

            // AF - Afghanistan
            { @"^(T6|YA)", "af" },

            // AG - Antigua and Barbuda
            { @"^V2", "ag" },

            // AL - Albania
            { @"^ZA", "al" },

            // AM - Armenia
            { @"^EK", "am" },

            // AN - Netherlands (formerly Netherlands Antilles)
            { @"^PJ", "an" },

            // AO - Angola
            { @"^D[23]", "ao" },

            // AR - Argentina
            { @"^((A[YZ])|(L[O-W])|(L[2-9]))", "ar" },

            // AT - Austria
            { @"^OE", "at" },

            // AU - Australia
            { @"^(AX|(V[H-N])|VZ)", "au" },

            // AW - Aruba
            { @"^P4", "aw" },

            // AZ - Azerbaijan
            { @"^4[J-K]", "az" },

            // BA - Bosnia and Herzagovina
            { @"^E7", "ba" },

            // TODO: BB
            { @"^8P", "bb" },

            // BD - Bangladesh
            { @"^S[23]", "bd" },

            // BE - Belgium
            { @"^O[N-T]", "be" },

            // BF - Burkina Faso
            { @"^XT", "bf" },

            // BG - Bulgaria
            { @"^LZ", "bg" },

            // BH - Bahrain
            { @"^A9", "bh" },

            // TODO: BI
            { @"^9U", "bi" },

            // BJ - Benin
            { @"^TY", "bj" },

            // BN - Brunei
            { @"^V8", "bn" },

            // BO - Bolivia
            { @"^CP", "bo" },

            // BR - Brazil
            { @"^((P[P-Y])|(Z[V-Z]))", "br" },

            // BS - The Bahamas
            { @"^C6", "bs" },

            // BT - Bhutan
            { @"^A5", "bt" },

            // BW - Botswana
            { @"^A2|8O", "bw" },

            // BY - Belarus
            { @"^E[U-W]", "by" },

            // BZ - Belize
            { @"^V3", "bz" },

            // CA - Canada
            { @"^((C[F-KYZ])|(V[A-GOXY])|(X[J-O]))", "bz" },

            // CD - Democratic Republic of the Congo
            { @"^9[O-T]", "cd" },

            // CF - Central African Republic
            { @"^TL", "cf" },

            // CG - Republic of the Congo
            { @"^TN", "cg" },

            // CH - Switzerland
            { @"^H[BE]", "ch" },

            // CI - Côte d'Ivoire/Ivory Coast
            { @"^TU", "ci" },

            // CK - Cook Islands
            { @"^E5", "ck" },

            // CL - Chile
            { @"^((C[A-E])|(X[QR]))", "cl" },

            // CM - Cameroon
            { @"^TJ", "cm" },

            // CN - People's Republic of China
            { @"^((B[A-LR-TYZ])|(XS))", "cn" },

            // CN/TW - Shared between PRC and Republic of China
            // TODO: Figure out a solution for these...
            //{ @"^3[H-U]", "" },

            // CO - Columbia
            { @"^((H[JK])|(5[JK]))", "co" },

            // CR - Costa Rica
            { @"^(TE|TI)", "cr" },

            // CU - Cuba
            { @"^((C[LMO])|T4)", "cu" },

            // CV - Cape Verde
            { @"^D4", "cv" },

            // CY - Cyprus
            { @"^(C4|H2|P3|5B)", "cy" },

            // CZ - Czechia/Czech Republic
            { @"^O[KL]", "cz" },

            // DE - Deutschland/Germany
            { @"^((D[A-R])|(Y[2-9]))", "de" },

            // DJ - Djibouti
            { @"^J2", "dj" },

            // DK - Denmark
            { @"^((O[U-Z])|XP|(5[PQ]))", "dk" },

            // DM - Dominica
            { @"^J7", "dm" },

            // DO - Dominican Republic
            { @"^HI", "do" },

            // DZ - Algeria
            { @"^7[RT-Y]", "dz" },

            // EC - Ecuador
            { @"^H[CD]", "ec" },

            // EE - Estonia
            { @"^ES", "ee" },

            // EG - Egypt
            { @"^((S((S[A-M])|U))|6[AB])", "eg" },

            // ER - Eritrea
            { @"^E3", "er" },

            // ES - Spain
            { @"^((A[MNO])|(E[A-H])", "es" },

            // ET - Ethiopia
            { @"^ET", "et" },

            // FI - Finland
            { @"^O[F-J]", "fi" },

            // FJ - Fiji
            { @"^3D[N-Z]", "fj" },

            // FM - Federated States of Micronesia
            { @"^FM", "fm" },

            // FR - France
            { @"^(F|(H[W-Y])|(T[HKMO-QV-X])", "fr" },

            // GA - Gabon
            { @"^TR", "ga" },

            // GB - United Kingdom
            { @"^(M|G|(V[PQS])|(Z[B-JN-OQ])|2)", "gb" },

            // GD - Grenada
            { @"^J3", "gd" },

            // 

            // US - United States
            { @"^[KN].*", "us" },

            // Arbitrary prefixes and/or country codes
            // OOKM - Sovereign Military Order of Malta/Knights of Malta
            { @"^1A", "ookm" },
            // OOLL Liberland
            { @"^1L", "ooll" },
        };

        /// <summary>
        /// string1: prefix pattern string
        /// string2: ISO 3166-1 alpha-2 country code
        /// </summary>
        private static IDictionary<string, Regex> prefixRegexCache = new Dictionary<string, Regex>();

        // "Borrows" some implementation from https://stackoverflow.com/a/11608874

        public static void TryGetRegion(string callsign, out string region)
        {
            string _output = null;

            foreach (KeyValuePair<string, string> rawPrefixPair in RawPrefixes)
            {
                string prefix = rawPrefixPair.Key;
                string cc = rawPrefixPair.Value;

                Regex regex;
                
                if (!prefixRegexCache.TryGetValue(prefix, out regex))
                {
                    regex = new Regex(prefix, RegexOptions.Compiled);
                    prefixRegexCache.Add(prefix, regex);
                }

                prefixRegexCache.TryGetValue(prefix, out regex);

                if (regex.Match(callsign).Success)
                {
                    _output = cc;
                    break;
                }
            }

            region = _output;
        }

        //public static string GetRegion(string callsign)
        //{
        //    foreach (KeyValuePair<string, string> prefix in RawStringPrefixes)
        //    {
        //        if (callsign.StartsWith(prefix.Key))
        //        {
        //            return prefix.Value;
        //        }
        //    }
        //}
    }
}
