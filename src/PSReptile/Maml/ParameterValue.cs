using System.Xml.Serialization;

namespace PSReptile.Maml
{
    /// <summary>
    ///     Represents a "parameterValue" element in a Powershell MAML help document.
    /// </summary>
    [XmlRoot("parameterValue", Namespace = Constants.XmlNamespace.Command)]
    public class ParameterValue
    {
        /// <summary>
        ///     The parameter data-type ("#text").
        /// </summary>
        [XmlText]
        public string DataType { get; set; }

        /// <summary>
        ///     Is the parameter mandatory?
        /// </summary>
        [XmlAttribute("required")]
        public bool IsMandatory { get; set; }

        /// <summary>
        ///     Is the parameter's data type variable-length?
        /// </summary>
        [XmlAttribute("variableLength")]
        public bool IsVariableLength { get; set; }
    }
}