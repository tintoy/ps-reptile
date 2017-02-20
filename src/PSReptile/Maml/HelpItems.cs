using System.Collections.Generic;
using System.Xml.Serialization;

namespace PSReptile.Maml
{
    /// <summary>
    ///     The root ("helpItems") XML element in a Powershell MAML help file.
    /// </summary>
    [XmlRoot("helpItems", Namespace = Constants.XmlNamespace.Root)]
    public class HelpItems
    {
        /// <summary>
        ///     Command documentation.
        /// </summary>
        [XmlArray("commands", Namespace = Constants.XmlNamespace.Command)]
        [XmlArrayItem("command", Namespace = Constants.XmlNamespace.Command)]
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}