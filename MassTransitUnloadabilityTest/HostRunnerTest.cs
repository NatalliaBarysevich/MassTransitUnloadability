using System;
using System.IO;
using System.Linq;
using MassTransitUnloadability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MassTransitUnloadabilityTest
{
    //Reproducible Demo
    [TestClass]
    public class HostRunnerTest
    {
        private FileInfo _dll = new FileInfo(@"..\..\..\..\ConsumerLib\bin\Debug\netstandard2.0\ConsumerLib.dll"); //provide extern library from current solution. This project not refer to it directly
        private bool ExternalAssemblyExists => AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "ConsumerLib");

        //when add some classes to service collection, it will be successes
        //should be start 1. because second test load assembly to domain, and it will not be correctly
        [TestMethod]
        public void _1_HostRunnerWithSimpleServicesTest()
        {
            UnloadAssemblyTest<HostRunnerWithSimpleServices>();
        }

        //when use masstransit extension test failed
        [TestMethod]
        public void _2_HostRunnerWithMassTransitTest()
        {
            UnloadAssemblyTest<HostRunnerWithMassTransit>();
        }

        private void UnloadAssemblyTest<T>() where T : AbstractHostRunner, new()
        {
            var loader = new T();
            Assert.IsFalse(ExternalAssemblyExists);
            loader.Create(_dll);
            Assert.IsTrue(ExternalAssemblyExists);
            loader.Start();
            loader.Stop();
            Assert.IsFalse(ExternalAssemblyExists, $"Extern assembly wasn't unload when use {typeof(T).Name}");
        }

    }
}
