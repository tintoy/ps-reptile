using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace PSReptile.Tests
{
    using Models.Maml;

    /// <summary>
    ///     Tests for MAML XML generation.
    /// </summary>
    public class XmlTests
    {
        /// <summary>
        ///     Generate MAML XML from a <see cref="Command"/>.
        /// </summary>
        [Fact]
        public void GenerateXml_From_Command()
        {
            XmlSerializerNamespaces namespacesWithPrefix = Constants.XmlNamespace.GetStandardPrefixes();
            XmlSerializer serializer = new XmlSerializer(typeof(Command));
            
            string actual;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(
                    writer,
                    new Command
                    {
                        Details =
                        {
                            Name = "Get-FooBar",
                            Verb = "Get",
                            Noun = "FooBar",
                            Synopsis =
                            {
                                "Retrieve one or more FooBars."
                            }
                        },
                        Description =
                        {
                            "This command works with FooBars.",
                            "It gets them.",
                            "I don't really know how to make it any clearer than that."
                        }
                    },
                    namespacesWithPrefix
                );
                writer.Flush();

                actual = writer.ToString();
            }
            
            Console.WriteLine(actual);
        }
    }
}
