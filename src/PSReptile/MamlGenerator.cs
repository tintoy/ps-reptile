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

            CmdletSynopsisAttribute cmdletSynopsisAttribute = cmdletTypeInfo.GetCustomAttribute<CmdletSynopsisAttribute>();
            CmdletDescriptionAttribute cmdletDescriptionAttribute = cmdletTypeInfo.GetCustomAttribute<CmdletDescriptionAttribute>();
            
            Command commandHelp = new Command
            {
                Details =
                {
                    Name = $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}",
                    Synopsis = ToParagraphs(
                        cmdletSynopsisAttribute?.Synopsis?.Trim() ?? String.Empty
                    ),
                    Verb = cmdletAttribute.VerbName,
                    Noun = cmdletAttribute.NounName
                },
                Description = ToParagraphs(
                    cmdletDescriptionAttribute?.Description?.Trim() ?? String.Empty
                )
            };

            // TODO: Handle multiple parameter sets.
            var parameterSets = new Dictionary<string, SyntaxItem>
            {
                // Default parameter set.
                [String.Empty] = new SyntaxItem
                {
                    CommandName = commandHelp.Details.Name
                }
            };

            foreach (PropertyInfo property in cmdletType.GetProperties().OrderBy(property => property.CanRead))
            {
                if (!Reflector.IsCmdletParameter(property))
                    continue;

                ParameterAttribute parameterAttribute = property.GetCustomAttributes<ParameterAttribute>().First();

                // TODO: Add support for localised help from resources.

                Parameter parameter = new Parameter
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
                };
                commandHelp.Parameters.Add(parameter);

                // Update command syntax for the current parameter set.
                string parameterSetName = parameterAttribute.ParameterSetName ?? String.Empty;
                
                SyntaxItem parameterSetSyntax;
                if (!parameterSets.TryGetValue(parameterSetName, out parameterSetSyntax))
                {
                    parameterSetSyntax = new SyntaxItem
                    {
                        CommandName = commandHelp.Details.Name
                    };
                    parameterSets.Add(parameterSetName, parameterSetSyntax);
                }

                parameterSetSyntax.Parameters.Add(parameter);
            }

            foreach (string parameterSetName in parameterSets.Keys.OrderBy(name => name))
            {
                commandHelp.Syntax.Add(
                    parameterSets[parameterSetName]
                );
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