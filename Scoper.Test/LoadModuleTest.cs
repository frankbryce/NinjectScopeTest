using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Modules;

namespace Scoper.Test
{
    public class TestModule : NinjectModule
    {
        public static string comparableString => "I'm comparable!";
        public override void Load()
        {
            Bind<IComparable>().ToConstant(comparableString);
        }
    }

    [TestClass]
    public class LoadModuleTest : AutoScopeTest
    {
        [TestInitialize]
        public void Setup()
        {
            Load(new TestModule());
        }

        [TestMethod]
        public void LoadingModulesWorks()
        {
            Assert.AreEqual(TestModule.comparableString, Get<IComparable>());
        }
    }
}
