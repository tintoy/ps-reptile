using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace PSReptile
{
    /// <summary>
    ///     Helper methods for reflecting on assemblies and types.
    /// </summary>
    public static class Reflector
    {
        /// <summary>
        ///     The base type for Powershell Cmdlets.
        /// </summary>
        public static readonly Type CmdletBaseType = typeof(Cmdlet);

        /// <summary>
        ///     Extended information about the base type for Powershell Cmdlets.
        /// </summary>
        public static readonly TypeInfo CmdletBaseTypeInfo = CmdletBaseType.GetTypeInfo();

        /// <summary>
        ///     Determine whether the specified type implements a Cmdlet.
        /// </summary>
        /// <param name="type">
        ///     The CLR <see cref="Type"/> to examine.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if the type implements a Cmdlet; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCmdlet(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            TypeInfo typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsPublic)
                return false;

            if (!typeInfo.IsClass)
                return false;

            if (typeInfo.IsAbstract)
                return false;
            
            if (!CmdletBaseType.IsAssignableFrom(type))
                return false;

            if (typeInfo.GetCustomAttribute<CmdletAttribute>() == null)
                return false;

            return true;
        }

        /// <summary>
        ///     Determine whether the specified property represents a Cmdlet parameter.
        /// </summary>
        /// <param name="property">
        ///     The property to examine.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if the property represents a Cmdlet parameter; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCmdletParameter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            // Public and writable.
            if (!property.CanWrite || !property.GetSetMethod().IsPublic)
                return false;

            // Must be decorated with [Parameter]
            if (!property.GetCustomAttributes<ParameterAttribute>().Any())
                return false;

            return true;
        }

        /// <summary>
        ///     Get all Cmdlet implementation types in the specified module assembly.
        /// </summary>
        /// <param name="moduleAssembly">
        ///     The assembly that implements a Powershell binary module.
        /// </param>
        /// <returns>
        ///     A sequence of types representing the Cmdlets.
        /// </returns>
        public static IEnumerable<Type> GetCmdletTypes(Assembly moduleAssembly)
        {
            if (moduleAssembly == null)
                throw new ArgumentNullException(nameof(moduleAssembly));

            foreach (Type type in moduleAssembly.GetTypes())
            {
                if (IsCmdlet(type))
                    yield return type;
            }
        }
    }
}