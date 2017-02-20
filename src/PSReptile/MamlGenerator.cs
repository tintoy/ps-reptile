using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace PSReptile
{
    using Maml;

    /// <summary>
    ///     Generates MAML from Powershell binary modules and the Cmdlets they contain.
    /// </summary>
    public static class MamlGenerator
    {
        /// <summary>
        ///     Generate MAML documentation for the specified module.
        /// </summary>
        /// <param name="moduleAssembly">
        ///     The assembly that implements the module.
        /// </param>
        /// <returns>
        ///     A <see cref="Command"/> representing the Cmdlet documentation.
        /// </returns>
        public static HelpItems Generate(Assembly moduleAssembly)
        {
            if (moduleAssembly == null)
                throw new ArgumentNullException(nameof(moduleAssembly));

            HelpItems help = new HelpItems();
            help.Commands.AddRange(
                Reflector.GetCmdletTypes(moduleAssembly).Select(
                    cmdletType => Generate(cmdletType)
                )
            );

            return help;
        }

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

            if (!Reflector.IsCmdlet(cmdletType))
                throw new ArgumentException($"'{cmdletType.FullName}' does not implement a Cmdlet (must be public, non-abstract, derive from '{typeof(Cmdlet).FullName}', and be decorated with '{typeof(CmdletAttribute).FullName}').", nameof(cmdletType));

            TypeInfo cmdletTypeInfo = cmdletType.GetTypeInfo();
            CmdletAttribute cmdletAttribute = cmdletTypeInfo.GetCustomAttribute<CmdletAttribute>();
            Debug.Assert(cmdletAttribute != null, "cmdletAttribute != null");

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
                if (!Reflector.IsCmdletParameter(property))
                    continue;

                // TODO: Handle multiple parameter sets.
                ParameterAttribute parameterAttribute = property.GetCustomAttributes<ParameterAttribute>().First();

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
            if (text == null)
                text = String.Empty;

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