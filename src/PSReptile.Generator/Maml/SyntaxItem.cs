using System.Collections.Generic;
using System.Xml.Serialization;

namespace PSReptile.Maml
{
    /// <summary>
    ///     Represents a "command:syntaxItem" element in a Powershell MAML help document.
    /// </summary>
    public class SyntaxItem
    {
        /// <summary>
        ///     The command name for this syntax.
        /// </summary>
        [XmlElement("name", Namespace = Constants.XmlNamespace.MAML, Order = 0)]
        public string CommandName { get; set; }

        /// <summary>
        ///     The command parameters associated with this syntax.
        /// </summary>
        [XmlElement("parameter", Namespace = Constants.XmlNamespace.Command, Order = 1)]
        public List<Parameter> Parameters = new List<Parameter>();
    }
}