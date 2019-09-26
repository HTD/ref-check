using System;
using System.Reflection;

namespace ReferencesCheck {

    /// <summary>
    /// Simple, equatable class to compare assembly basic properties.
    /// </summary>
    struct AssemblyVersion : IEquatable<AssemblyVersion> {

        /// <summary>
        /// Assembly short name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Assembly version.
        /// </summary>
        public Version Version;

        /// <summary>
        /// Creates the structure from <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyName"></param>
        public AssemblyVersion(AssemblyName assemblyName) {
            Name = assemblyName.Name;
            Version = assemblyName.Version;
        }

        /// <summary>
        /// Gets the hashcode for combined data.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(Name, Version);

        /// <summary>
        /// Equality test.
        /// </summary>
        /// <param name="other">Other element.</param>
        /// <returns>True if equal.</returns>
        public bool Equals(AssemblyVersion other) => Name.Equals(other.Name) && Version.Equals(other.Version);

        /// <summary>
        /// Gets the string representation.
        /// </summary>
        /// <returns>Assembly version as string.</returns>
        public override string ToString() => $"{Name} {Version}";

    }

}