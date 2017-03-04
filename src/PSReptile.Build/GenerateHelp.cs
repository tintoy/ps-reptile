using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace PSReptile.Build
{
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
        ///     Execute the task.
        /// </summary>
        /// <returns>
        ///     <c>true</c>, if the task executed succesfully; otherwise, <c>false</c>.
        /// </returns>
        public override bool Execute()
        {
            return true;
        }
    }
}