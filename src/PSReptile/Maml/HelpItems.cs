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
        ///     No idea what the point of this atribute is.
        /// </summary>
        [XmlAttribute("schema")]
        public string Schema { get; set; } = "maml";

        /// <summary>
        ///     Command documentation.
        /// </summary>
        [XmlElement("command", Namespace = Constants.XmlNamespace.Command)]
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}