using System;
using System.IO;
using System.Reflection;

namespace PSReptile.DotNetCli
{
    using Maml;
    using Utilities;

    /// <summary>
    ///     The main program entry-point.
    /// </summary>
    static class Program
    {
        /// <summary>
        ///     The main program entry-point.
        /// </summary>
        /// <param name="args">
        ///     Command-line arguments.
        /// </param>
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3 || args[0] != "gen-help")
            {
                Console.WriteLine("Usage:\n\tdotnet reptile gen-help <Module.dll> [Module.dll-Help.xml]");

                return;
            }

            try
            {
                string modulePath = Path.GetFullPath(args[1]);

                // If the module's dependencies are not available from
                // the usual places (e.g. dotnet-reptile's base directory)
                // then load them from the module directory.
                DirectoryAssemblyLoadContext loadContext = new DirectoryAssemblyLoadContext(
                    Path.GetDirectoryName(modulePath)
                );
                Assembly moduleAssembly = loadContext.LoadFromAssemblyPath(modulePath);
                HelpItems help = new MamlGenerator().Generate(moduleAssembly);

                FileInfo helpFile = new FileInfo(
                    fileName: args.Length == 3 ? Path.GetFullPath(args[2]) : modulePath + "-Help.xml"
                );
                if (helpFile.Exists)
                    helpFile.Delete();

                using (StreamWriter writer = helpFile.CreateText())
                {
                    help.WriteTo(writer);
                }

                Console.WriteLine($"Generated '{helpFile.FullName}'.");
            }
            catch (ReflectionTypeLoadException typeLoadError)
            {
                Console.WriteLine(typeLoadError);
                Console.WriteLine(
                    new String('=', 80)
                );
                foreach (Exception loaderException in typeLoadError.LoaderExceptions)
                    Console.WriteLine(loaderException);
            }
            catch (Exception unexpectedError)
            {
                Console.WriteLine(unexpectedError);
            }
        }
    }
}
