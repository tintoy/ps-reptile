using System.Collections.Generic;
using System.Xml.Serialization;

namespace PSReptile.Maml
{
    /// <summary>
    ///     Represents a "dev:type" element in a Powershell MAML help document.
    /// </summary>
    [XmlRoot("dataType", Namespace = Constants.XmlNamespace.Dev)]
    public class DataType
    {
        /// <summary>
        ///     The data-type name.
        /// </summary>
        [XmlElement("name", Namespace = Constants.XmlNamespace.Dev, Order = 0)]
        public string Name { get; set; }

        /// <summary>
        ///     The data-type URI (unused, for the most part).
        /// </summary>
        [XmlElement("uri", Namespace = Constants.XmlNamespace.MAML, Order = 1)]
        public string Uri { get; set; }

        /// <summary>
        ///     The data-type's detailed description (one or more paragraphs; "maml:description/maml:para").
        /// </summary>
        [XmlArray("description", Namespace = Constants.XmlNamespace.MAML, Order = 2)]
        [XmlArrayItem("para", Namespace = Constants.XmlNamespace.MAML)]
        public List<string> Description { get; set; } = new List<string>();
    }
}
