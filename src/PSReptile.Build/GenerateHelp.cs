using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace PSReptile.Build
{
    using Maml;
    using Utilities;

    /// <summary>
    ///     MSBuild task that generates help for a PowerShell Core binary module.
    /// </summary>
    public class GenerateHelp
        : Task
    {
        /// <summary>
        ///     The assembly that implements the PowerShell module.
        /// </summary>
        [Required]
        public ITaskItem ModuleAssembly { get; set; }

        /// <summary>
        ///     The help file to generate.
        /// </summary>
        [Required]
        public ITaskItem HelpFile { get; set; }

        /// <summary>
        ///     Execute the task.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if the task executed succesfully; otherwise, <c>false</c>.
        /// </returns>
        public override bool Execute()
        {
            FileInfo moduleAssemblyFile = new FileInfo(
                ModuleAssembly.GetMetadata("FullPath")
            );

            Log.LogMessage(MessageImportance.Low, "Scanning assembly '{0}'...",
                moduleAssemblyFile.FullName
            );

            DirectoryAssemblyLoadContext assemblyLoadContext = new DirectoryAssemblyLoadContext(
                fallbackDirectory: moduleAssemblyFile.Directory.FullName
            );

            HelpItems help = new HelpItems();
            MamlGenerator generator = new MamlGenerator();

            Assembly moduleAssembly = assemblyLoadContext.LoadFromAssemblyPath(moduleAssemblyFile.FullName);
            foreach (Type cmdletType in Reflector.GetCmdletTypes(moduleAssembly))
            {
                CmdletAttribute cmdletAttribute = cmdletType.GetTypeInfo().GetCustomAttribute<CmdletAttribute>();
                
                Log.LogMessage(MessageImportance.Low, "Generating help for cmdlet '{0}-{1}' ('{2}').",
                    cmdletAttribute.VerbName,
                    cmdletAttribute.NounName,
                    cmdletType.FullName
                );

                help.Commands.Add(
                    generator.Generate(cmdletType)
                );
            }

            FileInfo helpFile = new FileInfo(
                HelpFile.GetMetadata("FullPath")
            );
            if (helpFile.Exists)
                helpFile.Delete();

            using (StreamWriter writer = helpFile.CreateText())
            {
                help.WriteTo(writer);
            }

            Log.LogMessage(MessageImportance.Normal, "'{0}' -> '{1}'",
                moduleAssemblyFile.Name,
                helpFile.Name
            );

            return true;
        }
    }
}