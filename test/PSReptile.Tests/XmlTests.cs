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
            string expected = @"
<?xml version=""1.0"" encoding=""utf-16""?>
<command:command xmlns:maml=""http://schemas.microsoft.com/maml/2004/10"" xmlns:dev=""http://schemas.microsoft.com/maml/dev/2004/10"" xmlns:command=""http://schemas.microsoft.com/maml/dev/command/2004/10"">
  <command:details>
    <command:name>Get-FooBar</command:name>
    <command:verb>Get</command:verb>
    <command:noun>FooBar</command:noun>
    <maml:description>
      <maml:para>Retrieve one or more FooBars.</maml:para>
    </maml:description>
  </command:details>
  <maml:description>
    <maml:para>This command works with FooBars.</maml:para>
    <maml:para>It gets them.</maml:para>
    <maml:para>I don't really know how to make it any clearer than that.</maml:para>
  </maml:description>
</command:command>
            ".Trim();

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

            Assert.Equal(expected, actual);
        }
    }
}
