using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace PSReptile.Models.Maml
{
    /// <summary>
    ///     Represents a "command" element in a Powershell MAML help document.
    /// </summary>
    [XmlRoot("command", Namespace = Constants.XmlNamespace.Command)]
    public class Command
    {
        /// <summary>
        ///     The command details.
        /// </summary>
        [XmlElement("details", Namespace = Constants.XmlNamespace.Command, Form = XmlSchemaForm.Qualified, Order = 0)]
        public CommandDetails Details { get; set; } = new CommandDetails();

        /// <summary>
        ///     The command's detailed description (one or more paragraphs; "maml:description/maml:para").
        /// </summary>
        [XmlArray("description", Namespace = Constants.XmlNamespace.MAML, Form = XmlSchemaForm.Qualified, Order = 1)]
        [XmlArrayItem("para", Namespace = Constants.XmlNamespace.MAML, Form = XmlSchemaForm.Qualified)]
        public List<string> Description { get; set; } = new List<string>();

        // TODO: create types for the elements below.
        
        [XmlElement("syntax", Namespace = Constants.XmlNamespace.Command, Form = XmlSchemaForm.Qualified, Order = 2)]
        public string Syntax { get; set; }

        [XmlElement("parameters", Namespace = Constants.XmlNamespace.Command, Form = XmlSchemaForm.Qualified, Order = 3)]
        public string Parameters { get; set; }

        [XmlElement("inputTypes", Namespace = Constants.XmlNamespace.Command, Form = XmlSchemaForm.Qualified, Order = 4)]
        public string InputTypes { get; set; }

        [XmlElement("returnValues", Namespace = Constants.XmlNamespace.Command, Form = XmlSchemaForm.Qualified, Order = 5)]
        public string ReturnValues { get; set; }

        [XmlElement("alertSet", Namespace = Constants.XmlNamespace.MAML, Form = XmlSchemaForm.Qualified, Order = 6)]
        public string AlertSet { get; set; }

        [XmlElement("examples", Namespace = Constants.XmlNamespace.Command, Form = XmlSchemaForm.Qualified, Order = 7)]
        public string Examples { get; set; }

        [XmlElement("relatedLinks", Namespace = Constants.XmlNamespace.MAML, Form = XmlSchemaForm.Qualified, Order = 8)]
        public string RelatedLinks { get; set; }
    }
}