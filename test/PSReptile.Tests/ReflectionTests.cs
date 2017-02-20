using System;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace PSReptile.Tests
{
    /// <summary>
    ///     Tests for reflection on binary PowerShell modules.
    /// </summary>
    public class ReflectionTests
    {
        /// <summary>
        ///     The base class for Cmdlets.
        /// </summary>
        static Type CmdletBaseType = typeof(Cmdlet);

        /// <summary>
        ///     The primary assembly implementing the "PSReptile.SampleModule" PowerShell module.
        /// </summary>
        Assembly SampleModuleAssembly = Assembly.Load(
            new AssemblyName("PSReptile.SampleModule")
        );

        /// <summary>
        ///     Scan all classes in the sample module assembly that implement a Cmdlet.
        /// </summary>
        public void Get_Cmdlet_Classes()
        {
            Type[] cmdletTypes =
                SampleModuleAssembly.GetTypes()
                    .Select(type => type.GetTypeInfo())
                    .Where(type =>
                        type.IsPublic && type.IsClass && !type.IsAbstract
                        &&
                        type.GetCustomAttribute<CmdletAttribute>() != null
                    )
                    .Select(typeInfo => typeInfo.AsType())
                    .ToArray();

            Assert.Equal(1, cmdletTypes.Length);
        }
    }
}