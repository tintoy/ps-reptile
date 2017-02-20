using System;

namespace PSReptile
{
    /// <summary>
    ///     Provides inline help content for a Cmdlet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CmdletHelp
        : Attribute
    {
        /// <summary>
        ///     Provide inline help content for the Cmdlet.
        /// </summary>
        public CmdletHelp()
        {
        }

        /// <summary>
        ///     A brief description of the Cmdlet.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/bb525429.aspx">
        public string Synopsis { get; set; }

        /// <summary>
        ///     A detailed description of the Cmdlet.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/bb736332.aspx">
        public string Description { get; set; }
    }
}
