using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReferencesCheck {
    
    /// <summary>
    /// Main program class.
    /// </summary>
    class Program {

        /// <summary>
        /// Handles command line parameters and performs simplified solution analysis.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Exit code.</returns>
        static int Main(string[] args) { // ref-check x64
            if (!args.Any()) return Help(0);
            if (args.Length < 2 || args.Length > 3) return Help();
            var baseDirectory = args.Length == 3 ? args[2] : Directory.GetCurrentDirectory();
            if (!SupportedPlatforms.Match(args[0], out var platformDirectory) ||
                !SupportedConfigs.Match(args[1], out var configDirectory)) return Help();
            var solution = Directory.GetFiles(baseDirectory, "*.sln").SingleOrDefault();
            if (solution is null) return SolutionNotFound();
            Console.WriteLine($"{solution}:");
            Console.WriteLine();
            Console.WriteLine(Reference.GetConflicts(baseDirectory, configDirectory, platformDirectory, "\t"));
            return 0;
        }

        /// <summary>
        /// Displays help and returns 
        /// </summary>
        /// <param name="exitCode">Exit code, default -1.</param>
        /// <returns>Exit code.</returns>
        static int Help(int exitCode = -1) {
            var a = Assembly.GetExecutingAssembly();
            var v = a.GetName().Version;
            var exe = Path.GetFileNameWithoutExtension(a.Location);
            Console.WriteLine($"References Checker {v.Major}.{v.Minor} by CodeDog. Copyright (c) 2019. All rights reserved.");
            Console.WriteLine();
            Console.WriteLine("Description:");
            Console.WriteLine("\tDisplays possible references conflicts in a .NET solution.");
            Console.WriteLine("Usage:");
            Console.WriteLine($"\t{exe} [{SupportedPlatforms.Help}] [{SupportedConfigs.Help}] [optional-path]");
            return exitCode;
        }

        /// <summary>
        /// Displays solution not found error.
        /// </summary>
        /// <returns>Exit code.</returns>
        static int SolutionNotFound() {
            Console.WriteLine("Solution not found. Either use in directory with the solution, or specify the correct directory.");
            return -1;
        }

    }

}