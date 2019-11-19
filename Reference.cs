using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReferencesCheck {

    /// <summary>
    /// Reference analysis and handling.
    /// </summary>
    struct Reference : IEquatable<Reference> {

        /// <summary>
        /// Assembly referencing the other assembly.
        /// </summary>
        public AssemblyVersion In;

        /// <summary>
        /// Assembly being referenced.
        /// </summary>
        public AssemblyVersion Referenced;

        /// <summary>
        /// Creates assembly reference structure.
        /// </summary>
        /// <param name="assembly">Assembly referencing the other assebly.</param>
        /// <param name="referenced">Assembly being referenced.</param>
        public Reference(Assembly assembly, AssemblyName referenced) {
            In = new AssemblyVersion(assembly.GetName());
            Referenced = new AssemblyVersion(referenced);
        }

        /// <summary>
        /// Gets the solution references conflicts as formatted (indented) multiline string.
        /// </summary>
        /// <param name="baseDirectory">Base directory of the current solution.</param>
        /// <param name="currentConfig">Current configuration directory name.</param>
        /// <param name="currentPlatform">Current platform directory name.</param>
        /// <param name="indent">Indentation for each line.</param>
        /// <returns>Text.</returns>
        public static string GetConflicts(string baseDirectory, string currentConfig, string currentPlatform, string indent = "") {
            var targets = GetTargets(baseDirectory, currentConfig, currentPlatform);
            return GetConflicts(targets, indent);
        }

        /// <summary>
        /// Equality test.
        /// </summary>
        /// <param name="r">Other structure.</param>
        /// <returns></returns>
        public bool Equals(Reference r) =>
            r.In.Equals(In) && r.Referenced.Equals(Referenced);

        /// <summary>
        /// Gets the hash code for equality tests.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode() => HashCode.Combine(In, Referenced);

        /// <summary>
        /// Gets the string representation of the reference.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[ {Referenced} ] referenced by [ {In.Name} ]";


        /// <summary>
        /// Gets all references in base directory.
        /// </summary>
        /// <param name="baseDirectory">Base directory of the current solution.</param>
        /// <param name="currentConfig">Current configuration directory name.</param>
        /// <param name="currentPlatform">Current platform directory name.</param>
        /// <returns>All target references.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Assembly load exception reason is perfectly irrelevant here")]
        private static IEnumerable<Reference> GetTargets(string baseDirectory, string currentConfig, string currentPlatform) {
            var directories =
                Directory.GetDirectories(baseDirectory, currentConfig, SearchOption.AllDirectories)
                .Where(i => !((currentPlatform == "x64" && i.Contains("x86") || (currentPlatform == "x86" && i.Contains("x64")))));
            if (!directories.Any()) return Array.Empty<Reference>();
            var files =
                directories.Select(i => Directory.GetFiles(i, "*.dll").Concat(Directory.GetFiles(i, "*.exe"))).Aggregate((a, b) => a.Concat(b));
            var assemblies =
                files.Select(i => {
                    try {
                        return Assembly.LoadFile(i);
                    }
                    catch {
                        return null;
                    }
                }).Where(i => !(i is null));
            var noVersion = new Version(0, 0, 0, 0);
            var references =
                assemblies.SelectMany(
                    assembly =>
                        assembly.GetReferencedAssemblies()
                        .Select(referencedAssembly => new Reference(assembly, referencedAssembly))
                )
                .Where(i => i.In.Version != noVersion && i.Referenced.Version != noVersion); // yep, i found such evil.
            return references;
        }

        /// <summary>
        /// Gets all references conflicts (same name, different version).
        /// </summary>
        /// <param name="references">Assembly references.</param>
        /// <returns>Conflict groups.</returns>
        private static IEnumerable<IGrouping<string, Reference>> GetConflicts(IEnumerable<Reference> references)
            => from reference in references
               group reference by reference.Referenced.Name
                       into referenceGroup
               where referenceGroup.Select(reference => reference.Referenced.GetHashCode()).Distinct().Count() > 1
               select referenceGroup;

        /// <summary>
        /// Gets all references conflicts (same name, different version). As pre-formatted string.
        /// </summary>
        /// <param name="references">Assembly references.</param>
        /// <param name="indent">Prefix added before each line (indentation).</param>
        /// <returns>Conflicts as string.</returns>
        private static string GetConflicts(IEnumerable<Reference> references, string indent) {
            var groups = GetConflicts(references);
            if (!groups.Any()) return $"{indent}No conflicts.{Environment.NewLine}";
            var builder = new StringBuilder();
            foreach (var group in groups) {
                builder.Append(indent);
                builder.Append("[x] ");
                builder.Append(group.Key);
                builder.AppendLine(":");
                foreach (var item in group.Distinct()) {
                    builder.Append(indent);
                    builder.Append('\t');
                    builder.AppendLine(item.ToString());
                }
            }
            return builder.ToString();
        }

    }

}