using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace PSReptile.Utilities
{
    /// <summary>
    ///     XML documentation extracted from comments.
    /// </summary>
    class XmlDoc
    {
        /// <summary>
        ///     Member documentation elements, keyed by member name.
        /// </summary>
        readonly Dictionary<string, XElement> _memberDocumentation = new Dictionary<string, XElement>();

        /// <summary>
        ///     Wrap the specified XML.
        /// </summary>
        /// <param name="xml">
        ///     The documentation XML.
        /// </param>
        public XmlDoc(XDocument xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            IEnumerable<XElement> memberElements = xml.Elements("doc").Elements("members").Elements("member");
            foreach (XElement memberElement in memberElements)
            {
                string memberName = (string)memberElement.Attribute("name");
                if (String.IsNullOrWhiteSpace(memberName))
                    continue;

                _memberDocumentation.Add(memberName, memberElement);
            }
        }

        /// <summary>
        ///     Get the summary (if any) for the specified member.
        /// </summary>
        /// <param name="member">
        ///     The member to examine.
        /// </param>
        /// <returns>
        ///     The member summary, or <c>null</c> if no summary was found for the specified member.
        /// </returns>
        public string GetSummary(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            XElement memberSummary = GetDocumentation(member)?.Element("summary");
            if (memberSummary == null)
                return null;

            return memberSummary.Value?.Trim();
        }

        /// <summary>
        ///     Get the remarks (if any) for the specified member.
        /// </summary>
        /// <param name="member">
        ///     The member to examine.
        /// </param>
        /// <returns>
        ///     The member remarks, or <c>null</c> if no remarks were found for the specified member.
        /// </returns>
        public string GetRemarks(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            XElement memberRemarks = GetDocumentation(member)?.Element("remarks");
            if (memberRemarks == null)
                return null;

            return memberRemarks.Value?.Trim();
        }

        /// <summary>
        ///     Get the documentation (if any) for the specified member.
        /// </summary>
        /// <param name="member">
        ///     The member to examine.
        /// </param>
        /// <returns>
        ///     An <see cref="XElement"/> containing the member documentation.
        /// </returns>
        public XElement GetDocumentation(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            string memberName = GetMemberName(member);

            XElement documentation;
            if (_memberDocumentation.TryGetValue(memberName, out documentation))
                return documentation;

            return null;
        }

        /// <summary>
        ///     Determine the name used to identify the specified member in the documentation XML.
        /// </summary>
        /// <param name="member">
        ///     The member to examine.
        /// </param>
        /// <returns>
        ///     The member name.
        /// </returns>
        string GetMemberName(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                {
                    TypeInfo type = (TypeInfo)member;

                    return $"T:{type.FullName}";
                }
                case MemberTypes.Property:
                {
                    PropertyInfo property = (PropertyInfo)member;

                    return $"P:{property.DeclaringType.FullName}.{property.Name}";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(member), member, $"Unexpected member type '{member.MemberType}'.");
                }
            }
        }
    }
}