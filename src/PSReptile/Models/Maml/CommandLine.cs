using System.Xml.Serialization;

namespace PSReptile.Models.Maml
{
    /// <summary>
    ///     Represents a "command" element in a Powershell MAML help document.
    /// </summary>
    [XmlRoot("commandLine", Namespace = Constants.XmlNamespace.Command)]
    public class CommandLine
    {
        /// <summary>
        ///     The command text.
        /// </summary>
        [XmlElement("commandText", Namespace = Constants.XmlNamespace.Command)]
        public string CommandText { get; set; }
    }
}
