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

            // BB - Barbados
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

            // BI - Burundi
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
            { @"^((A[MNO])|(E[A-H]))", "es" },

            // ET - Ethiopia
            { @"^ET", "et" },

            // FI - Finland
            { @"^O[F-J]", "fi" },

            // FJ - Fiji
            { @"^3D[N-Z]", "fj" },

            // FM - Federated States of Micronesia
            { @"^FM", "fm" },

            // FR - France
            { @"^(F|(H[W-Y])|(T[HKMO-QV-X]))", "fr" },

            // GA - Gabon
            { @"^TR", "ga" },

            // GB - United Kingdom
            { @"^(M|G|(V[PQS])|(Z[B-JN-OQ])|2)", "gb" },

            // GD - Grenada
            { @"^J3", "gd" },

            // GE - Georgia
            { @"^4L", "ge" },

            // GH - Ghana
            { @"^9G", "gh" },

            // GM - The Gambia
            { @"^C5", "gm" },

            // GN - Guinea
            { @"^3X", "gn" },

            // GQ - Equatorial Guinea
            { @"^3C", "gq" },

            // GR - Greece
            { @"^((J4)|(S[V-Z]))", "gr" },

            // GT - Guatemala
            { @"^T[DG]", "gt" },

            // GW - Guinea-Bissau
            { @"^J5", "gw" },

            // GY - Guyana
            { @"^8R", "gy" },

            // HK - Hong Kong
            { @"^VR", "hk" },

            // HN - Honduras
            { @"^H[QR]", "hn" },

            // HR - Croatia
            { @"^9A", "hr" },

            // HT - Haiti
            { @"^(HH|4V)", "ht" },

            // HU - Hungary
            { @"^H[AG]", "hu" },

            // ID - Indonesia
            { @"^((JZ)|(P[K-O])|(Y[B-H])|([78][A-I]))", "id" },

            // IE - Ireland
            { @"^E[IJ]", "ie" },

            // IL - Israel
            { @"^4[XZ]", "il" },

            // IN - India
            { @"^(([AV][T-W])|(8[T-Y]))", "in" },

            // IQ - Iraq
            { @"^((HN)|(YI))", "iq" },

            // IR - Iran
            { @"^((E[PQ])|(9[B-D]))", "ir" },

            // IS - Iceland
            { @"^TF", "is" },

            // IT - Italy
            { @"^I", "it" },

            // JM - Jamaica
            { @"^6Y", "jm" },

            // JO - Jordan
            { @"^JY", "jo" },

            // JP - Japan
            { @"^((J[A-S])|([78][J-N]))", "jp" },

            // KE - Kenya
            { @"^5[YZ]", "ke" },

            // KG - Kyrgyzstan
            { @"^EX", "kg" },

            // KH - Cambodia
            { @"^XU", "kh" },

            // KI - Kiribati
            { @"^T3", "ki" },

            // KM - Comoros
            { @"^D6", "km" },

            // KN - Saint Kitts and Nevis
            { @"^V4", "kn" },

            // KP - North Korea
            { @"^((HM)|(P[5-9]))", "kp" },

            // KR - South Korea
            { @"^((D[ST7-9])|(HL)|(6[K-N]))", "kr" },

            // KW - Kuwait
            { @"^9K", "kw" },

            // KZ - Kazakhstan
            { @"U[N-Q]", "kz" },

            // LA - Laos
            { @"^XW", "la" },

            // LB - Lebanon
            { @"^LB", "lb" },

            // LC - Saint Lucia
            { @"^J6", "lc" },

            // LI - Liechtenstein
            { @"^HB(0|(3Y)|L)", "li" },

            // LK - Sri Lanka
            { @"^4[P-S]", "lk" },

            // LR - Liberia
            { @"^((A8)|(D5)|(EL)|(5[LM])|(6Z))", "lr" },

            // LS - Lesotho
            { @"^7P", "ls" },

            // LT - Lithuania
            { @"^LY", "lt" },

            // LU - Luxembourg
            { @"^LX", "lu" },

            // LV - Latvia
            { @"^YL", "lv" },

            // LY - Libya
            { @"^5A", "ly" },

            // MA - Morocco
            { @"^((CN)|(5[C-G]))", "ma" },

            // MC - Monaco
            { @"^3A", "mc" },

            // MD - Moldova
            { @"^ER", "md" },

            // ME - Montenegro
            { @"^4O", "me" },

            // MG - Mongolia
            { @"^((J[TUV])|(5[RS])|(6X))", "mg" },

            // MH - Marshall Islands
            { @"^V7", "mh" },

            // MK - North Macedonia
            { @"^Z3", "mk" },

            // ML - Mali
            { @"^TZ", "ml" },

            // MM - Myanmar
            { @"^X[YZ]", "mm" },

            // MO - Macao
            { @"^XX", "mo" },

            // MR - Mauritania
            { @"^5T", "mr" },

            // MT - Malta
            { @"^9H", "mt" },

            // MU - Mauritius
            { @"^3B", "mu" },

            // MV - Maldives
            { @"^8Q", "mv" },

            // MW - Malawi
            { @"^7Q", "mw" },

            // MX - Mexico
            { @"^((X[A-I])|(4[A-C])|(6[D-J]))", "mx" },

            // MY - Malaysia
            { @"^9[MW]", "my" },

            // MZ - Mozambique
            { @"^C[89]", "mz" },

            // NA - Namibia
            { @"^V5", "na" },

            // NE - Niger
            { @"^5U", "ne" },

            // NG - Nigeria
            { @"^5[NO]", "ng" },

            // NI - Nicaragua
            { @"^((H[T67])|(YN))", "ni" },

            // NL - Netherlands
            { @"^P[A-I]", "nl" },

            // NO - Norway
            { @"^((J[WX])|(L[A-N])|(3Y))", "no" },

            // NP - Nepal
            { @"^9N", "np" },

            // NR - Nauru
            { @"^C2", "nr" },

            // NU - Niue
            { @"^E6", "nu" },

            // NZ - New Zealand
            { @"^Z[K-M]", "nz" },

            // OM - Oman
            { @"^A4", "om" },

            // PA - Panama
            { @"^((H[OP389])|(3[EF]))", "pa" },

            // PE - Peru
            { @"^((O[A-C])|(4T))", "pe" },

            // PG - Papua New Guinea
            { @"^P2", "pg" },

            // PH - Philippines
            { @"((D[U-Z])|(4[D-I]))", "ph" },

            // PK - Pakistan
            { @"^((A[P-R])|(6[P-S]))", "pk" },

            // PL - Poland
            { @"^((HF)|(S[N-R])|(3Z))", "pl" },

            // PS - Palestinian Authority
            { @"^E4", "ps" },

            // PT - Portugal
            { @"^C[Q-U]", "pt" },

            // PW - Palau
            { @"^T8", "pw" },

            // PY - Paraguay
            { @"^ZP", "py" },

            // QA - Qatar
            { @"^A7", "qa" },

            // RO - Romania
            { @"^Y[O-R]", "ro" },

            // RS - Serbia
            { @"^Y[TU]", "rs" },

            // RU - Russia
            { @"^(R|(U[A-I]))", "ru" },

            // RW - Rwanda
            { @"^9X", "rw" },

            // SA - Saudi Arabia
            { @"^[H78]Z", "sa" },

            // SB - Solomon Islands
            { @"^H4", "sb" },

            // SC - Seychelles
            { @"^S7", "sc" },

            // SD - Sudan
            { @"^S((S[N-Z])|T|(6[TU]))", "sd" },

            // SE - Sweden
            { @"^((S[A-M])|([78]S))", "se" },

            // SG - Singapore
            { @"^(S6|9V)", "sg" },

            // SI - Slovenia
            { @"^S5", "si" },

            // SK - Slovakia
            { @"^OM", "sk" },

            // SL - Sierra Leone
            { @"^9L", "sl" },

            // SM - San Marino
            { @"^T7", "sm" },

            // SN - Senegal
            { @"^6[VW]", "sn" },

            // SO - Somalia
            { @"^(T5|6O)", "so" },

            // SR - Suriname
            { @"^PZ", "sr" },

            // SS - South Sudan
            { @"^Z8", "ss" },

            // ST - São Tomé and Príncipe
            { @"^S9", "st" },

            // SV - El Salvador
            { @"^(HU|YS)", "sv" },

            // SY - Syria
            { @"^(YK|6C)", "sy" },

            // SZ - Eswatini
            { @"^3D[A-M]", "sz" },

            // TD - Chad
            { @"^TT", "td" },

            // TG - Togo
            { @"^5V", "tg" },

            // TH - Thailand
            { @"^(E2|HS)", "th" },

            // TJ - Tajikistan
            { @"^EY", "tj" },

            // TL - Timor-Leste
            { @"^4W", "tl" },

            // TM - Turkmenistan
            { @"^EZ", "tm" },

            // TN - Tunisia
            { @"^(TS|3V)", "tn" },

            // TO - Tonga
            { @"^A3", "to" },

            // TR - Turkey
            { @"^((T[A-C])|YM)","tr" },

            // TT - Trinidad and Tobago
            { @"^9[YZ]", "tt" },

            // TV - Tuvalu
            { @"^T2", "tv" },

            // TW - Taiwan/Republic of China
            { @"^B[M-QU-X]", "tw" },

            // TZ - Tanzania, United Republic of
            { @"^5[HI]", "tz" },

            // UA - Ukraine
            { @"^((E[MNO])|(U[R-Z]))", "ua" },

            // UG - Uganda
            { @"^5X", "ug" },

            // US - United States
            { @"^((A[A-L])|[KNW])", "us" },

            // UY - Uruguay
            { @"^C[V-X]", "uy" },

            // UZ - Uzbekistan
            { @"^U[J-M]", "uz" },

            // VA - Vatican City
            { @"^HV", "va" },
            // FATHER DE HVPONTIFF K
            // Anyone else imagining the Pope as a ham?

            // VC - Saint Vincent and the Grenadines
            { @"^J8", "vc" },

            // VE - Venezuela
            { @"^((Y[V-Y])|4M)", "ve" },

            // VN - Vietnam
            { @"^(XV|3W)", "vn" },

            // VU - Vanuatu
            { @"^YJ", "vu" },

            // WS - Samoa
            { @"^5W", "ws" },

            // YE - Yemen
            { @"^7O", "ye" },

            // ZA - South Africa
            { @"^(S8|(Z[R-U]))", "za" },

            // ZM - Zambia
            { @"^9[IJ]", "zm" },

            // ZW - Zimbabwe
            { @"^Z2", "zw" },

            // International organizations with their own allocations
            // XA - ICAO/International Civil Aviation Organization
            { @"^4Y", "xa" },

            // XM - World Meteorological Organization
            { @"^C7", "xm" },

            // XU - UN/United Nations
            { @"^4U", "xu" },

            // Unofficial prefixes used in disputed territories
            // or in nation states without an ITU prefix.
            // ITU does not issue prefixes containing 0 or 1 (zero or one).

            /*
             * Because ISO 3166-1 alpha-2 doesn't specify all of these regions,
             * these regions are specified by the arbitrary-use codes.
             * These codes (minus what ITU uses for the ICAO (XA), the World
             * Meteorological Organization (XM), and the UN (XU)) are AA,
             * Q[M-Z], X[B-LN-TV-Z], and ZZ. In addition, "OO" (two "O" U+004F or "o" U+004F) is available
             * as an escape code to specify existing two-letter combinations, like OOUS or oo.
             */

            // OOKM - Sovereign Military Order of Malta/Knights of Malta
            { @"^1A", "ookm" },
            
            // OOLL - Liberland
            { @"^1L", "ooll" },

            // OOWS - Western Sahara
            { @"^S0", "oows" },
        };

        /// <summary>
        /// A cache of the compiled prefix patterns so it doesn't have to recompile on each call.
        /// </summary>
        // string: prefix pattern string
        // Regex: compiled pattern string
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
                    regex = new Regex(prefix, RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
