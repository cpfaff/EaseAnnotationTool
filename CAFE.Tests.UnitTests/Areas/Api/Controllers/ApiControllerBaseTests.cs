using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAFE.Tests.UnitTests.Areas.Api.Controllers
{
    [TestClass()]
    public class ApiControllerBaseTests
    {
        [TestMethod()]
        public void ApiControllerBaseTest()
        {

        }

        [TestMethod()]
        public void ExecuteSuccessTest()
        {
            ////Arrange
            //var loggerMock = new Mock<ILogger>();
            //var securityServiceMock = new Mock<ISecurityService>();

            //ApiControllerBase baseC = new ApiControllerBase(
            //    loggerMock.Object,
            //    securityServiceMock.Object);

            //bool successRaised = false;

            ////Act
            //baseC.Execute(() => new List<User>(), users =>
            //{
            //    successRaised = true;
            //}, exception =>
            //{
            //    successRaised = false;
            //});

            ////Assert
            //Assert.IsTrue(successRaised);
        }

        [TestMethod()]
        public void ExecuteFailTest()
        {
            ////Arrange
            //var loggerMock = new Mock<ILogger>();
            //var securityServiceMock = new Mock<ISecurityService>();

            //ApiControllerBase baseC = new ApiControllerBase(
            //    loggerMock.Object,
            //    securityServiceMock.Object);

            //bool successFailed = false;

            ////Act
            //baseC.Execute<List<User>>(() =>
            //{
            //    throw new ApplicationException();
            //}, users =>
            //{
            //    successFailed = false;
            //}, exception =>
            //{
            //    successFailed = true;
            //});

            ////Assert
            //Assert.IsTrue(successFailed);
        }

        [TestMethod(), ExpectedException(typeof(ApplicationException))]
        public void ExecuteExceptionTest()
        {
            ////Arrange
            //var loggerMock = new Mock<ILogger>();
            //var securityServiceMock = new Mock<ISecurityService>();

            //ApiControllerBase baseC = new ApiControllerBase(
            //    loggerMock.Object,
            //    securityServiceMock.Object);

            ////Act
            //baseC.Execute<List<User>>(() =>
            //{
            //    throw new ApplicationException();
            //}, users =>
            //{
            //}, exception =>
            //{
            //}, true);

            ////Assert
            //Assert.Fail("Test must raise exception");
            throw new ApplicationException();
        }
    }
}