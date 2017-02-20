using System;
using System.IO;
using System.Reflection;

namespace PSReptile.DotNetCli
{
    using System.Xml.Serialization;
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
            if (args.Length != 1)
            {
                Console.WriteLine("Usage:\n\tdotnet-reptile MyModule.dll");

                return;
            }

            try
            {
                DirectoryAssemblyLoadContext loadContext = new DirectoryAssemblyLoadContext(
                    Path.GetDirectoryName(
                        Path.GetFullPath(args[0])
                    )
                );
                Assembly moduleAssembly = loadContext.LoadFromAssemblyPath(args[0]);
                HelpItems help = MamlGenerator.Generate(moduleAssembly);

                using (StringWriter writer = new StringWriter())
                {
                    new XmlSerializer(typeof(HelpItems)).Serialize(
                        writer, help, Constants.XmlNamespace.GetStandardPrefixes()
                    );
                    writer.Flush();

                    Console.WriteLine(
                        writer.ToString()
                    );
                }
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
