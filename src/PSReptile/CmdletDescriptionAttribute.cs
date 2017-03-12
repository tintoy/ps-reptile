using System;

namespace PSReptile
{
    /// <summary>
    ///     Provides an inline detailed description in the help content for a Cmdlet.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/bb736332.aspx"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CmdletDescriptionAttribute
        : Attribute
    {
        /// <summary>
        ///     Provides an inline detailed description for the Cmdlet.
        /// </summary>
        public CmdletDescriptionAttribute(string description)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));

            Description = description;
        }

        /// <summary>
        ///     A detailed description of the Cmdlet.
        /// </summary>
        public string Description { get; }
    }
}
