using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace PSReptile.Utilities
{
    /// <summary>
    ///     An assembly load context that falls back to a specific directory for assemblies that cannot be found by other means.
    /// </summary>
    /// <remarks>
    ///     Ignores assembly version / public key token (for now, that's acceptable, but we may want to tighten it up later).
    /// </remarks>
    public class DirectoryAssemblyLoadContext
        : AssemblyLoadContext
    {
        /// <summary>
        ///     The target directory.
        /// </summary>
        DirectoryInfo _fallbackDirectory;

        /// <summary>
        ///     Create a new <see cref="DirectoryAssemblyLoadContext"/>.
        /// </summary>
        /// <param name="fallbackDirectory">
        ///     The fallback directory for assemblies.
        /// </param>
        public DirectoryAssemblyLoadContext(string fallbackDirectory)
        {
            if (String.IsNullOrWhiteSpace(fallbackDirectory))
                throw new ArgumentException("Must specify a valid target directory.", nameof(fallbackDirectory));

            _fallbackDirectory = new DirectoryInfo(fallbackDirectory);
        }

        /// <summary>
        ///     Create a new <see cref="DirectoryAssemblyLoadContext"/>.
        /// </summary>
        /// <param name="fallbackDirectory">
        ///     The fallback directory for assemblies.
        /// </param>
        public DirectoryAssemblyLoadContext(DirectoryInfo fallbackDirectory)
        {
            if (fallbackDirectory == null)
                throw new ArgumentNullException(nameof(fallbackDirectory));

            _fallbackDirectory = fallbackDirectory;
        }

        /// <summary>
        ///     Attempt to load the specified assembly.
        /// </summary>
        /// <param name="assembly">
        ///     An <see cref="AssemblyName"/> representing the assembly to load.
        /// </param>
        /// <returns>
        ///     The assembly, or <c>null</c> if no matching assembly was found.
        /// </returns>
        protected override Assembly Load(AssemblyName assembly)
        {
            // Try load our own copy, first.
            try
            {
                return Assembly.Load(assembly);
            }
            catch (FileNotFoundException)
            {
                // Nope? Ok, let's fall back to the target directory.
            }

            FileInfo assemblyFile = new FileInfo(
                Path.Combine(_fallbackDirectory.FullName, assembly.Name + ".dll")
            );
            if (!assemblyFile.Exists)
                return null;

            try
            {
                return LoadFromAssemblyPath(assemblyFile.FullName);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
        }

        /// <summary>
        ///     Create a <see cref="DirectoryAssemblyLoadContext"/> targeting the directory where the specified assembly file is located.
        /// </summary>
        /// <param name="assemblyFilePath">
        ///     The full path to the assembly file.
        /// </param>
        /// <returns>
        ///     The configured <see cref="DirectoryAssemblyLoadContext"/>.
        /// </returns>
        public static DirectoryAssemblyLoadContext FromAssemblyFile(string assemblyFilePath)
        {
            if (String.IsNullOrWhiteSpace(assemblyFilePath))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'assemblyFile'.", nameof(assemblyFilePath));
            
            FileInfo assemblyFile = new FileInfo(assemblyFilePath);
            if (!assemblyFile.Exists)
                throw new FileNotFoundException($"Cannot find assembly '{assemblyFilePath}'.", assemblyFilePath);

            return new DirectoryAssemblyLoadContext(
                fallbackDirectory: assemblyFile.Directory.FullName
            );
        }
    }
}