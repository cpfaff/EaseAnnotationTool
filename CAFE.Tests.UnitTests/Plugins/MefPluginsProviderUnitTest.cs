
using System.Linq;
using CAFE.Core.Plugins;
using CAFE.Services.Plugins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAFE.Tests.UnitTests.Plugins
{
    [TestClass]
    public class MefPluginsProviderUnitTest
    {

        [TestInitialize]
        public void Setup()
        {

        }

        [TestMethod]
        public void LoadPluginsTest()
        {
            //Arrange
            IPluginsProvider provider = new MefPluginsProvider();

            //Act
            var plugins = provider.Sources;

            //Assert
            Assert.IsTrue(plugins.Any());
        }
    }
}
