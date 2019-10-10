using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace MassTransitUnloadability
{
    /// <summary>
    /// Collectable assembly load context https://docs.microsoft.com/en-us/dotnet/standard/assembly/unloadability
    /// </summary>
    class TestAssemblyLoadContext : AssemblyLoadContext
    {
        public TestAssemblyLoadContext() : base(isCollectible: true)
        {
        }

        protected override Assembly Load(AssemblyName name)
        {
            return null;
        }
    }
}
