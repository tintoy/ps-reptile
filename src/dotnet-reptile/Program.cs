using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace PSReptile.DotNetCli
{
    using Maml;

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
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage:\n\tdotnet-reptile <Module.dll> [Module.dll-Help.xml]");

                return;
            }

            try
            {
                string modulePath = Path.GetFullPath(args[0]);

                DirectoryAssemblyLoadContext loadContext = new DirectoryAssemblyLoadContext(
                    Path.GetDirectoryName(modulePath)
                );
                Assembly moduleAssembly = loadContext.LoadFromAssemblyPath(modulePath);
                HelpItems help = MamlGenerator.Generate(moduleAssembly);

                FileInfo helpFile = new FileInfo(
                    fileName: args.Length == 2 ? Path.GetFullPath(args[1]) : modulePath + "-Help.xml"
                );
                if (helpFile.Exists)
                    helpFile.Delete();

                using (StreamWriter writer = helpFile.CreateText())
                {
                    new XmlSerializer(typeof(HelpItems)).Serialize(
                        writer, help, Constants.XmlNamespace.GetStandardPrefixes()
                    );
                    writer.Flush();
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
