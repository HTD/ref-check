using System;
using System.Linq;

namespace ReferencesCheck {

    /// <summary>
    /// Supported project configurations.
    /// </summary>
    static class SupportedConfigs {

        /// <summary>
        /// Supported names.
        /// </summary>
        private static readonly string[] Names = new string[] { "Debug", "Release" };

        /// <summary>
        /// Gets the help string for supported names.
        /// </summary>
        public static string Help => $"({String.Join("|", Names.Select(i => $"\"{i.ToLowerInvariant()}\""))})";

        /// <summary>
        /// Matches a command line parameter for configuration name.
        /// </summary>
        /// <param name="config">Configuration name given.</param>
        /// <param name="configDirectory">Configuration directory name.</param>
        /// <returns>True if given name matches.</returns>
        public static bool Match(string config, out string configDirectory) {
            configDirectory = null;
            foreach (var name in Names) {
                if (name.Equals(config, StringComparison.OrdinalIgnoreCase)) {
                    configDirectory = name;
                    return true;
                }
            }
            return false;
        }


    }

}
