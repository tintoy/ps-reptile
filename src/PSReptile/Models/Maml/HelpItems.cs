using System.Collections.Generic;
using System.Xml.Serialization;

namespace PSReptile.Models.Maml
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
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}