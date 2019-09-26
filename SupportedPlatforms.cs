using System;
using System.Linq;

namespace ReferencesCheck {

    /// <summary>
    /// Supported project platforms.
    /// </summary>
    static class SupportedPlatforms {

        /// <summary>
        /// Supported platforms.
        /// </summary>
        private static readonly string[] Names = new string[] { "x64", "x86", "all" };

        /// <summary>
        /// Gets the help string for supported platforms.
        /// </summary>
        public static string Help => $"({String.Join("|", Names.Select(i => $"\"{i}\""))})";

        /// <summary>
        /// Matches a command line parameter for platform name.
        /// </summary>
        /// <param name="platform">Platform name given.</param>
        /// <param name="platformDirectory">Platform directory name. "" for "all".</param>
        /// <returns>True if given name matches.</returns>
        public static bool Match(string platform, out string platformDirectory) {
            var key = platform.ToLowerInvariant();
            platformDirectory = null;
            if (!Names.Contains(key)) return false;
            platformDirectory = key;
            if (key == Names[2]) platformDirectory = "";
            return true;
        }

    }

}