using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace PSReptile
{
    using Models.Maml;

    /// <summary>
    ///     Generates MAML from Powershell binary modules and the Cmdlets they contain.
    /// </summary>
    public static class MamlGenerator
    {
        /// <summary>
        ///     The base type for Powershell Cmdlets.
        /// </summary>
        static readonly Type CmdletBaseType = typeof(Cmdlet);

        /// <summary>
        ///     Extended information about the base type for Powershell Cmdlets.
        /// </summary>
        static readonly TypeInfo CmdletBaseTypeInfo = CmdletBaseType.GetTypeInfo();

        /// <summary>
        ///     Generate MAML documentation for the specified Cmdlet.
        /// </summary>
        /// <param name="cmdletType">
        ///     The CLR type that implements the Cmdlet.
        /// </param>
        /// <returns>
        ///     A <see cref="Command"/> representing the Cmdlet documentation.
        /// </returns>
        public static Command Generate(Type cmdletType)
        {
            if (cmdletType == null)
                throw new ArgumentNullException(nameof(cmdletType));

            TypeInfo cmdletTypeInfo = cmdletType.GetTypeInfo();

            if (!CmdletBaseTypeInfo.IsAssignableFrom(cmdletTypeInfo))
                throw new ArgumentException($"'{cmdletType.FullName}' does not derive from '{CmdletBaseType.FullName}'.", nameof(cmdletType));

            CmdletAttribute cmdletAttribute = cmdletTypeInfo.GetCustomAttribute<CmdletAttribute>();
            if (cmdletAttribute == null)
                throw new ArgumentException($"'{cmdletType.FullName}' is missing the '{typeof(CmdletAttribute).FullName}' custom attribute.", nameof(cmdletType));

            CmdletHelpAttribute cmdletHelpAttribute = cmdletTypeInfo.GetCustomAttribute<CmdletHelpAttribute>();
            
            Command commandHelp = new Command
            {
                Details =
                {
                    Name = $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}",
                    Synopsis = ToParagraphs(
                        cmdletHelpAttribute?.Synopsis?.Trim() ?? String.Empty
                    ),
                    Verb = cmdletAttribute.VerbName,
                    Noun = cmdletAttribute.NounName
                },
                Description = ToParagraphs(
                    cmdletHelpAttribute?.Description?.Trim() ?? String.Empty
                )
            };

            foreach (PropertyInfo property in cmdletType.GetProperties())
            {
                // Public and writable.
                if (!property.CanWrite || !property.GetSetMethod().IsPublic)
                    continue;

                // Must be decorated with [Parameter]
                ParameterAttribute parameterAttribute = property.GetCustomAttribute<ParameterAttribute>();
                if (parameterAttribute == null)
                    continue;

                // TODO: Add support for localised help from resources.
                commandHelp.Parameters.Add(new Parameter
                {
                    Name = property.Name,
                    Description = ToParagraphs(parameterAttribute.HelpMessage),
                    Value =
                    {
                        IsMandatory = parameterAttribute.Mandatory,
                        DataType = property.PropertyType.Namespace == "System" ? // This is a PowerShell convention.
                            property.PropertyType.Name
                            :
                            property.PropertyType.FullName,
                    },
                    IsMandatory = parameterAttribute.Mandatory,
                    SupportsGlobbing = property.GetCustomAttribute<SupportsWildcardsAttribute>() != null
                });
            }

            return commandHelp;
        }

        /// <summary>
        ///     Split text into paragraphs.
        /// </summary>
        /// <param name="text">
        ///     The text to split.
        /// </param>
        /// <returns>
        ///     A list of paragraphs.
        /// </returns>
        static List<string> ToParagraphs(string text)
        {
            return new List<string>(
                text.Split(
                    separator: new[] {
                        Environment.NewLine
                    },
                    options: StringSplitOptions.None
                ).Select(
                    line => line.Trim()
                )
            );
        }
    }
}