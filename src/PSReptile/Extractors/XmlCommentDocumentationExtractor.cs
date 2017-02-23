using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace PSReptile.Extractors
{
    using Utilities;

    /// <summary>
    ///     Cmdlet documentation extractor that obtains documentation from XML documentation comments.
    /// </summary>
    public class XmlCommentDocumentationExtractor
        : IDocumentationExtractor
    {
        /// <summary>
        ///     Cached XML for documentation comments.
        /// </summary>
        readonly Dictionary<Assembly, XmlDoc> _documentationCache = new Dictionary<Assembly, XmlDoc>();

        /// <summary>
        ///     Extract the synopsis for a Cmdlet.
        /// </summary>
        /// <param name="cmdletType">
        ///     The CLR type that implements the Cmdlet.
        /// </param>
        /// <returns>
        ///     The synopsis, or <c>null</c> if no synopsis could be extracted by this extractor.
        /// 
        ///     An empty string means a synopsis was extracted, but the synopsis is empty (this is legal).
        /// </returns>
        public string GetCmdletSynopsis(TypeInfo cmdletType)
        {
            if (cmdletType == null)
                throw new ArgumentNullException(nameof(cmdletType));

            XmlDoc assemblyDoc = GetAssemblyDocumentation(cmdletType);
            if (assemblyDoc == null)
                return null;

            return assemblyDoc.GetSummary(cmdletType);
        }

        /// <summary>
        ///     Extract the synopsis for a Cmdlet.
        /// </summary>
        /// <param name="cmdletType">
        ///     The CLR type that implements the Cmdlet.
        /// </param>
        /// <returns>
        ///     The description, or <c>null</c> if no description could be extracted by this extractor.
        /// 
        ///     An empty string means a description was extracted, but the description is empty (this is legal).
        /// </returns>
        public string GetCmdletDescription(TypeInfo cmdletType)
        {
            if (cmdletType == null)
                throw new ArgumentNullException(nameof(cmdletType));

            XmlDoc assemblyDoc = GetAssemblyDocumentation(cmdletType);
            if (assemblyDoc == null)
                return null;

            return assemblyDoc.GetRemarks(cmdletType);
        }

        /// <summary>
        ///     Extract the description for a Cmdlet parameter.
        /// </summary>
        /// <param name="parameterProperty">
        ///     The property that represents the parameter.
        /// </param>
        /// <returns>
        ///     The description, or <c>null</c> if no description could be extracted by this extractor.
        /// 
        ///     An empty string means a description was extracted, but the description is empty (this is legal).
        /// </returns>
        public string GetParameterDescription(PropertyInfo parameterProperty)
        {
            if (parameterProperty == null)
                throw new ArgumentNullException(nameof(parameterProperty));

            XmlDoc assemblyDoc = GetAssemblyDocumentation(parameterProperty);
            if (assemblyDoc == null)
                return null;

            return assemblyDoc.GetSummary(parameterProperty);
        }

        /// <summary>
        ///     Retrieve the documentation (if any) for the specified property's declaring type's assembly.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <returns>
        ///     The documentation XML (as an <see cref="XmlDoc"/>), or <c>null</c> if no documentation was found for the property's declaring type's assembly.
        /// </returns>
        XmlDoc GetAssemblyDocumentation(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return GetAssemblyDocumentation(property.DeclaringType);
        }

        /// <summary>
        ///     Retrieve the documentation (if any) for the specified type's assembly.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <returns>
        ///     The documentation XML (as an <see cref="XmlDoc"/>), or <c>null</c> if no documentation was found for the type's assembly.
        /// </returns>
        XmlDoc GetAssemblyDocumentation(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetAssemblyDocumentation(type.GetTypeInfo());
        }

        /// <summary>
        ///     Retrieve the documentation (if any) for the specified type's assembly.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <returns>
        ///     The documentation XML (as an <see cref="XmlDoc"/>), or <c>null</c> if no documentation was found for the type's assembly.
        /// </returns>
        XmlDoc GetAssemblyDocumentation(TypeInfo type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetAssemblyDocumentation(type.Assembly);
        }

        /// <summary>
        ///     Retrieve the documentation (if any) for the specified assembly.
        /// </summary>
        /// <param name="assembly">
        ///     The target assembly.
        /// </param>
        /// <returns>
        ///     The documentation XML (as an <see cref="XmlDoc"/>), or <c>null</c> if no documentation was found for the target assembly.
        /// </returns>
        XmlDoc GetAssemblyDocumentation(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            XmlDoc documentation;
            if (!_documentationCache.TryGetValue(assembly, out documentation))
            {
                FileInfo documentationFile = new FileInfo(
                    Path.ChangeExtension(assembly.Location, ".xml")
                );
                if (!documentationFile.Exists)
                    return null;

                using (Stream documentationStream = documentationFile.OpenRead())
                {
                    documentation = new XmlDoc(
                        XDocument.Load(documentationStream)
                    );
                }
                _documentationCache.Add(assembly, documentation);
            }

            return documentation;
        }
    }
}