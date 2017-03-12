using System.Collections.Generic;
using System.Xml.Serialization;

namespace PSReptile.Maml
{
    /// <summary>
    ///     Represents a "parameter" element in a Powershell MAML help document.
    /// </summary>
    [XmlRoot("parameter", Namespace = Constants.XmlNamespace.Command)]
    public class Parameter
    {
        /// <summary>
        ///     The parameter name ("maml:name").
        /// </summary>
        [XmlElement("name", Namespace = Constants.XmlNamespace.MAML, Order = 0)]
        public string Name { get; set; }

        /// <summary>
        ///     The parameter's detailed description (one or more paragraphs; "maml:description/maml:para").
        /// </summary>
        [XmlArray("description", Namespace = Constants.XmlNamespace.MAML, Order = 1)]
        [XmlArrayItem("para", Namespace = Constants.XmlNamespace.MAML)]
        public List<string> Description { get; set; } = new List<string>();

        /// <summary>
        ///     The parameter value information ("command:parameterValue").
        /// </summary>
        [XmlElement("parameterValue", Namespace = Constants.XmlNamespace.Command, Order = 2)]
        public ParameterValue Value { get; set; } = new ParameterValue();

        /// <summary>
        ///     Is the parameter mandatory?
        /// </summary>
        [XmlAttribute("required")]
        public bool IsMandatory { get; set; }

        /// <summary>
        ///     Does the parameter support globbing (wildcards)?
        /// </summary>
        [XmlAttribute("globbing")]
        public bool SupportsGlobbing { get; set; }

        /// <summary>
        ///     Can the parameter accept its value from the pipeline?
        /// </summary>
        /// <remarks>
        ///     Either "true (ByValue)", "true (ByPropertyName)", or "false".
        /// </remarks>
        [XmlAttribute("pipelineInput")]
        public PipelineInputType SupportsPipelineInput { get; set; }

        /// <summary>
        ///     The parameter's position.
        /// </summary>
        /// <remarks>
        ///     Either "named", or the 0-based position of the parameter.
        /// </remarks>
        [XmlAttribute("position")]
        public string Position { get; set; } = "named";
    }
}