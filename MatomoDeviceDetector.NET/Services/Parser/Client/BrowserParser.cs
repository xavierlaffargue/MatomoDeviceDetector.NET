// --------------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowserParser.cs" company="Agile Flex Agency">
// Copyright © 2000-2020 by Agile Flex Agency. All rights reserved. Website: https://agile-flex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------------

namespace MatomoDeviceDetectorNET.Services.Parser.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MatomoDeviceDetectorNET.Services.Client;
    using MatomoDeviceDetectorNET.Services.Parser.Client.Browser;
    using MatomoDeviceDetectorNET.Services.Parser.Client.Browser.Engine;
    using MatomoDeviceDetectorNET.Services.Results;
    using MatomoDeviceDetectorNET.Services.Results.Client;

    /// <summary>
    /// Browser Parser.
    /// </summary>
    public class BrowserParser : ClientParserAbstract<List<Services.Client.Browser>, BrowserMatchResult>
    {
        /// <summary>
        /// Known browsers mapped to their internal short codes.
        /// </summary>
        private static Dictionary<string, string> availableBrowsers = new Dictionary<string, string>
        {
            { "1B", "115 Browser" },
            { "2B", "2345 Browser" },
            { "36", "360 Phone Browser" },
            { "3B", "360 Browser" },
            { "AA", "Avant Browser" },
            { "AB", "ABrowse" },
            { "AF", "ANT Fresco" },
            { "AG", "ANTGalio" },
            { "AL", "Aloha Browser" },
            { "AH", "Aloha Browser Lite" },
            { "AM", "Amaya" },
            { "AO", "Amigo" },
            { "AN", "Android Browser" },
            { "AE", "AOL Desktop" },
            { "AD", "AOL Shield" },
            { "AR", "Arora" },
            { "AX", "Arctic Fox" },
            { "AV", "Amiga Voyager" },
            { "AW", "Amiga Aweb" },
            { "A0", "Atom" },
            { "AT", "Atomic Web Browser" },
            { "AS", "Avast Secure Browser" },
            { "VG", "AVG Secure Browser" },
            { "BA", "Beaker Browser" },
            { "BM", "Beamrise" },
            { "BB", "BlackBerry Browser" },
            { "BD", "Baidu Browser" },
            { "BS", "Baidu Spark" },
            { "BI", "Basilisk" },
            { "BE", "Beonex" },
            { "BH", "BlackHawk" },
            { "BJ", "Bunjalloo" },
            { "BL", "B-Line" },
            { "BU", "Blue Browser" },
            { "BN", "Borealis Navigator" },
            { "BR", "Brave" },
            { "BK", "BriskBard" },
            { "BX", "BrowseX" },
            { "BZ", "Browzar" },
            { "CA", "Camino" },
            { "CL", "CCleaner" },
            { "C0", "Centaury" },
            { "CC", "Coc Coc" },
            { "C2", "Colibri" },
            { "CD", "Comodo Dragon" },
            { "C1", "Coast" },
            { "CX", "Charon" },
            { "CE", "CM Browser" },
            { "CF", "Chrome Frame" },
            { "HC", "Headless Chrome" },
            { "CH", "Chrome" },
            { "CI", "Chrome Mobile iOS" },
            { "CK", "Conkeror" },
            { "CM", "Chrome Mobile" },
            { "CN", "CoolNovo" },
            { "CO", "CometBird" },
            { "CB", "COS Browser" },
            { "CP", "ChromePlus" },
            { "CR", "Chromium" },
            { "CY", "Cyberfox" },
            { "CS", "Cheshire" },
            { "CT", "Crusta" },
            { "CZ", "Crazy Browser" },
            { "CU", "Cunaguaro" },
            { "CV", "Chrome Webview" },
            { "DB", "dbrowser" },
            { "DE", "Deepnet Explorer" },
            { "DT", "Delta Browser" },
            { "DF", "Dolphin" },
            { "DO", "Dorado" },
            { "DL", "Dooble" },
            { "DI", "Dillo" },
            { "DD", "DuckDuckGo Privacy Browser" },
            { "EC", "Ecosia" },
            { "EI", "Epic" },
            { "EL", "Elinks" },
            { "EB", "Element Browser" },
            { "EE", "Elements Browser" },
            { "EZ", "eZ Browser" },
            { "EU", "EUI Browser" },
            { "EP", "GNOME Web" },
            { "ES", "Espial TV Browser" },
            { "FA", "Falkon" },
            { "FX", "Faux Browser" },
            { "F1", "Firefox Mobile iOS" },
            { "FB", "Firebird" },
            { "FD", "Fluid" },
            { "FE", "Fennec" },
            { "FF", "Firefox" },
            { "FK", "Firefox Focus" },
            { "FY", "Firefox Reality" },
            { "FR", "Firefox Rocket" },
            { "FL", "Flock" },
            { "FM", "Firefox Mobile" },
            { "FW", "Fireweb" },
            { "FN", "Fireweb Navigator" },
            { "FU", "FreeU" },
            { "GA", "Galeon" },
            { "GH", "Ghostery Privacy Browser" },
            { "GB", "Glass Browser" },
            { "GE", "Google Earth" },
            { "GO", "GOG Galaxy" },
            { "HA", "Hawk Turbo Browser" },
            { "HO", "hola! Browser" },
            { "HJ", "HotJava" },
            { "HU", "Huawei Browser" },
            { "IB", "IBrowse" },
            { "IC", "iCab" },
            { "I2", "iCab Mobile" },
            { "I1", "Iridium" },
            { "I3", "Iron Mobile" },
            { "I4", "IceCat" },
            { "ID", "IceDragon" },
            { "IV", "Isivioo" },
            { "IW", "Iceweasel" },
            { "IE", "Internet Explorer" },
            { "IM", "IE Mobile" },
            { "IR", "Iron" },
            { "JB", "Japan Browser" },
            { "JS", "Jasmine" },
            { "JI", "Jig Browser" },
            { "JP", "Jig Browser Plus" },
            { "JO", "Jio Browser" },
            { "KB", "K.Browser" },
            { "KI", "Kindle Browser" },
            { "KM", "K-meleon" },
            { "KO", "Konqueror" },
            { "KP", "Kapiko" },
            { "KN", "Kinza" },
            { "KW", "Kiwi" },
            { "KD", "Kode Browser" },
            { "KY", "Kylo" },
            { "KZ", "Kazehakase" },
            { "LB", "Cheetah Browser" },
            { "LF", "LieBaoFast" },
            { "LG", "LG Browser" },
            { "LH", "Light" },
            { "LI", "Links" },
            { "LO", "Lovense Browser" },
            { "LU", "LuaKit" },
            { "LL", "Lulumi" },
            { "LS", "Lunascape" },
            { "LN", "Lunascape Lite" },
            { "LX", "Lynx" },
            { "M1", "mCent" },
            { "MB", "MicroB" },
            { "MC", "NCSA Mosaic" },
            { "MZ", "Meizu Browser" },
            { "ME", "Mercury" },
            { "MF", "Mobile Safari" },
            { "MI", "Midori" },
            { "MO", "Mobicip" },
            { "MU", "MIUI Browser" },
            { "MS", "Mobile Silk" },
            { "MN", "Minimo" },
            { "MT", "Mint Browser" },
            { "MX", "Maxthon" },
            { "NM", "MxNitro" },
            { "MY", "Mypal" },
            { "MR", "Monument Browser" },
            { "MW", "MAUI WAP Browser" },
            { "NR", "NFS Browser" },
            { "NB", "Nokia Browser" },
            { "NO", "Nokia OSS Browser" },
            { "NV", "Nokia Ovi Browser" },
            { "NX", "Nox Browser" },
            { "NE", "NetSurf" },
            { "NF", "NetFront" },
            { "NL", "NetFront Life" },
            { "NP", "NetPositive" },
            { "NS", "Netscape" },
            { "NT", "NTENT Browser" },
            { "OC", "Oculus Browser" },
            { "O1", "Opera Mini iOS" },
            { "OB", "Obigo" },
            { "OD", "Odyssey Web Browser" },
            { "OF", "Off By One" },
            { "HH", "OhHai Browser" },
            { "OE", "ONE Browser" },
            { "OX", "Opera GX" },
            { "OG", "Opera Neon" },
            { "OH", "Opera Devices" },
            { "OI", "Opera Mini" },
            { "OM", "Opera Mobile" },
            { "OP", "Opera" },
            { "ON", "Opera Next" },
            { "OO", "Opera Touch" },
            { "OS", "Ordissimo" },
            { "OR", "Oregano" },
            { "O0", "Origin In-Game Overlay" },
            { "OY", "Origyn Web Browser" },
            { "OV", "Openwave Mobile Browser" },
            { "OW", "OmniWeb" },
            { "OT", "Otter Browser" },
            { "PL", "Palm Blazer" },
            { "PM", "Pale Moon" },
            { "PY", "Polypane" },
            { "PP", "Oppo Browser" },
            { "PR", "Palm Pre" },
            { "PU", "Puffin" },
            { "PW", "Palm WebPro" },
            { "PA", "Palmscape" },
            { "PX", "Phoenix" },
            { "PB", "Phoenix Browser" },
            { "PO", "Polaris" },
            { "PT", "Polarity" },
            { "PI", "PrivacyWall" },
            { "PS", "Microsoft Edge" },
            { "Q1", "QQ Browser Mini" },
            { "QQ", "QQ Browser" },
            { "QT", "Qutebrowser" },
            { "QU", "Quark" },
            { "QZ", "QupZilla" },
            { "QM", "Qwant Mobile" },
            { "QW", "QtWebEngine" },
            { "RE", "Realme Browser" },
            { "RK", "Rekonq" },
            { "RM", "RockMelt" },
            { "SB", "Samsung Browser" },
            { "SA", "Sailfish Browser" },
            { "S8", "Seewo Browser" },
            { "SC", "SEMC-Browser" },
            { "SE", "Sogou Explorer" },
            { "SF", "Safari" },
            { "S5", "Safe Exam Browser" },
            { "SW", "SalamWeb" },
            { "SH", "Shiira" },
            { "S1", "SimpleBrowser" },
            { "SY", "Sizzy" },
            { "SK", "Skyfire" },
            { "SS", "Seraphic Sraf" },
            { "SL", "Sleipnir" },
            { "S6", "Slimjet" },
            { "7S", "7Star" },
            { "LE", "Smart Lenovo Browser" },
            { "SN", "Snowshoe" },
            { "SO", "Sogou Mobile Browser" },
            { "S2", "Splash" },
            { "SI", "Sputnik Browser" },
            { "SR", "Sunrise" },
            { "SP", "SuperBird" },
            { "SU", "Super Fast Browser" },
            { "S3", "surf" },
            { "SG", "Stargon" },
            { "S0", "START Internet Browser" },
            { "S4", "Steam In-Game Overlay" },
            { "ST", "Streamy" },
            { "SX", "Swiftfox" },
            { "SZ", "Seznam Browser" },
            { "TO", "t-online.de Browser" },
            { "TA", "Tao Browser" },
            { "TF", "TenFourFox" },
            { "TB", "Tenta Browser" },
            { "TZ", "Tizen Browser" },
            { "TU", "Tungsten" },
            { "TG", "ToGate" },
            { "TS", "TweakStyle" },
            { "TV", "TV Bro" },
            { "UB", "UBrowser" },
            { "UC", "UC Browser" },
            { "UM", "UC Browser Mini" },
            { "UT", "UC Browser Turbo" },
            { "UR", "UR Browser" },
            { "UZ", "Uzbl" },
            { "VI", "Vivaldi" },
            { "VV", "vivo Browser" },
            { "VB", "Vision Mobile Browser" },
            { "VM", "VMware AirWatch" },
            { "WI", "Wear Internet Browser" },
            { "WP", "Web Explorer" },
            { "WE", "WebPositive" },
            { "WF", "Waterfox" },
            { "WH", "Whale Browser" },
            { "WO", "wOSBrowser" },
            { "WT", "WeTab Browser" },
            { "YJ", "Yahoo! Japan Browser" },
            { "YA", "Yandex Browser" },
            { "YL", "Yandex Browser Lite" },
            { "YN", "Yaani Browser" },
            { "YB", "Yolo Browser" },
            { "XI", "Xiino" },
            { "XV", "Xvast" },
            { "ZV", "Zvu" },
        };

        /// <summary>
        /// Browser families mapped to the short codes of the associated browsers.
        /// </summary>
        private static Dictionary<string, string[]> browserFamilies = new Dictionary<string, string[]>
        {
            {
                "Android Browser", new[]
                {
                    "AN", "MU",
                }
            },
            {
                "BlackBerry Browser", new[]
                {
                    "BB",
                }
            },
            {
                "Baidu", new[]
                {
                    "BD", "BS",
                }
            },
            {
                "Amiga",  new[]
                {
                    "AV", "AW",
                }
            },
            {
                "Chrome", new[]
                {
                    "CH", "BA", "BR", "CC", "CD", "CM", "CI", "CF", "CN",
                    "CR", "CP", "DD", "IR", "RM", "AO", "TS", "VI", "PT",
                    "AS", "TB", "AD", "SB", "WP", "I3", "CV", "WH", "SZ",
                    "QW", "LF", "KW", "2B", "CE", "EC", "MT", "MS", "HA",
                    "OC", "MZ", "BM", "KN", "SW", "M1", "FA", "TA", "AH",
                    "CL", "SU", "EU", "UB", "LO", "VG", "TV", "A0", "1B",
                    "S4", "EE", "AE", "VM", "O0", "TG", "GB", "SY", "HH",
                    "YJ", "LL", "TU", "XV", "C2", "QU", "YN", "JB", "MR",
                    "S6", "7S", "NM", "PB", "UR", "NR", "SG", "S8",
                }
            },
            {
                "Firefox", new[]
                {
                    "FF", "FE", "FM", "SX", "FB", "PX", "MB", "EI", "WF",
                    "CU", "TF", "QM", "FR", "I4", "GZ", "MO", "F1", "BI",
                    "MN", "BH", "TO", "OS", "MY", "FY", "AX", "C0", "LH",
                    "S5", "ZV", "IW", "PI", "BN",
                }
            },
            {
                "Internet Explorer", new[]
                {
                    "IE", "IM", "PS", "CZ", "BZ",
                }
            },
            {
                "Konqueror", new[]
                {
                    "KO",
                }
            },
            {

                "NetFront", new[]
                {
                    "NF",
                }
            },
            {
                "NetSurf", new[]
                {
                    "NE",
                }
            },
            {
                "Nokia Browser", new[]
                {
                    "NB", "NO", "NV", "DO",
                }
            },
            {
                "Opera", new[]
                {
                    "OP", "OM", "OI", "ON", "OO", "OG", "OH", "O1", "OX",
                }
            },
            {
                "Safari", new[]
                {
                    "SF", "MF", "SO",
                }
            },
            {
                "Sailfish Browser", new[]
                {
                    "SA",
                }
            },
        };

        /// <summary>
        /// Browsers that are available for mobile devices only.
        /// </summary>
        private static string[] mobileOnlyBrowsers =
        {
            "36", "OC", "PU", "SK", "MF", "OI", "OM", "DD", "DB",
            "ST", "BL", "IV", "FM", "C1", "AL", "SA", "SB", "FR",
            "WP", "HA", "NX", "HU", "VV", "RE", "CB", "MZ", "UM",
            "FK", "FX", "WI", "MN", "M1", "AH", "SU", "EU", "EZ",
            "UT", "DT", "S0", "QU", "YN", "JB", "GH", "PI", "SG",
            "KD",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserParser"/> class.
        /// </summary>
        public BrowserParser()
        {
            this.FixtureFile = "regexes/client/browsers.yml";
            this.ParserName = "browser";
            this.RegexList = this.GetRegexes();
        }

        /// <summary>
        /// Returns list of all available browsers.
        /// </summary>
        /// <returns>Returns dictionary.</returns>
        public static Dictionary<string, string> GetAvailableBrowsers()
        {
            return availableBrowsers;
        }

        /// <summary>
        /// Returns list of all available browser families.
        /// </summary>
        /// <returns>Returns Browser Families.</returns>
        public static Dictionary<string, string[]> GetAvailableBrowserFamilies()
        {
            return browserFamilies;
        }

        /// <summary>
        /// Get the Browser Family.
        /// </summary>
        /// <param name="browserLabel">Label.</param>
        /// <param name="name">Name.</param>
        /// <returns>Return bool.</returns>
        public static bool GetBrowserFamily(string browserLabel, out string name)
        {
            foreach (var family in browserFamilies)
            {
                if (!family.Value.Contains(browserLabel))
                {
                    continue;
                }

                name = family.Key;
                return true;
            }

            name = "Unknown";
            return false;
        }

        /// <summary>
        ///  Returns if the given browser is mobile only.
        /// </summary>
        /// <param name="browser">Label or name of browser.</param>
        /// <returns>Returns bool.</returns>
        public static bool IsMobileOnlyBrowser(string browser)
        {
            return mobileOnlyBrowsers.Contains(browser) || (availableBrowsers.ContainsKey(browser) && mobileOnlyBrowsers.Contains(availableBrowsers[browser]));
        }

        /// <summary>
        /// Public Parse.
        /// </summary>
        /// <returns>Returns Match.</returns>
        public override ParseResult<BrowserMatchResult> Parse()
        {
            var result = new ParseResult<BrowserMatchResult>();
            Services.Client.Browser localBrowser = null;
            string[] localMatches = null;

            foreach (var browser in this.RegexList)
            {
                var matches = this.MatchUserAgent(browser.Regex);

                if (matches.Length > 0)
                {
                    localBrowser = browser;
                    localMatches = matches;
                    break;
                }
            }

            if (localMatches != null)
            {
                var name = this.BuildByMatch(localBrowser.Name, localMatches);

                foreach (var availableBrowser in availableBrowsers)
                {
                    if (string.Equals(name, availableBrowser.Value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        var version = this.BuildVersion(localBrowser.Version, localMatches);
                        var engine = this.BuildEngine(localBrowser.Engine ?? new Engine(), version);
                        var engineVersion = this.BuildEngineVersion(engine);

                        result.Add(new BrowserMatchResult
                        {
                            Type = this.ParserName,
                            Name = name,
                            Version = version,
                            ShortName = availableBrowser.Key,
                            Engine = engine,
                            EngineVersion = engineVersion,
                        });
                    }
                }
            }

            return result;
            throw new Exception("Detected browser name was not found in AvailableBrowsers");
        }

        /// <summary>
        /// Build Engine.
        /// </summary>
        /// <param name="engineData">Engine Data.</param>
        /// <param name="browserVersion">Browser Version.</param>
        /// <returns>Returns Match.</returns>
        protected string BuildEngine(Engine engineData, string browserVersion)
        {
            var engine = string.Empty;

            if (!string.IsNullOrEmpty(engineData.Default))
            {
                engine = engineData.Default;
            }

            // check if engine is set for browser version
            if (engineData.Versions != null && engineData.Versions.Count > 0)
            {
                foreach (var version in engineData.Versions)
                {
                    if (string.IsNullOrEmpty(browserVersion))
                    {
                        continue;
                    }

                    var ver = !version.Key.Contains(".") ? version.Key + ".0" : version.Key;

                    if (browserVersion.EndsWith(".", StringComparison.Ordinal))
                    {
                        browserVersion = browserVersion.TrimEnd('.');
                    }

                    browserVersion = !browserVersion.Contains(".") ? browserVersion + ".0" : browserVersion;

                    if (new Version(browserVersion).CompareTo(new Version(ver)) >= 0)
                    {
                        engine = version.Value;
                    }
                }
            }

            if (string.IsNullOrEmpty(engine))
            {
                var engineParser = new EngineParser();
                engineParser.SetUserAgent(this.UserAgent);
                var engineResult = engineParser.Parse();

                if (engineResult.Success)
                {
                    engine = engineResult.Match.Name;
                }
            }

            return engine;
        }

        /// <summary>
        /// Build Engine Version.
        /// </summary>
        /// <param name="engine">The Engine.</param>
        /// <returns>Return Match.</returns>
        protected string BuildEngineVersion(string engine)
        {
            var engineVersion = new VersionParser(this.UserAgent, engine);
            var result = engineVersion.Parse();
            return result.Success ? result.Match.Name : string.Empty;
        }
    }
}