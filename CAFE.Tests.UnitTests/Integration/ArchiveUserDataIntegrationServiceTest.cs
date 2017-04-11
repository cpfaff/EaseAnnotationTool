using System;
using System.Linq.Expressions;
using AutoMapper;
//using AutoMapper;
using CAFE.Core.Integration;
using CAFE.Core.Resources;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.DAL.Models.Resources;
using CAFE.Services.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IConfigurationProvider = CAFE.Core.Configuration.IConfigurationProvider;

namespace CAFE.Tests.UnitTests.Integration
{
    [TestClass]
    public class ArchiveUserDataIntegrationServiceTest
    {
        Mock<IAnnotationItemIntegrationService> _annotationItemIntegrationServiceMock;
        Mock<IRepository<DbUser>> _usersRepositoryMock;
        Mock<IRepository<DbAnnotationItemAccessibleUsers>> _annotationUsersRepositoryMock;
        Mock<IRepository<DbAnnotationItemAccessibleGroups>> _annotationGroupsRepositoryMock;
        Mock<IRepository<DbAnnotationItem>> _annotationItemRepositoryMock;
        Mock<IRepository<DbVocabularyUserValue>> _usersVocabularyRepositoryMock;
        Mock<IConfigurationProvider> _configurationProviderMock;
        Mock<IVocabularyService> _vocabularyServiceMock;
        Mock<ISecurityService> _userFilesServiceMock;

        [TestInitialize]
        public void Setup()
        {
            Mapper.Initialize(Services.Mapping.MapperConfig.Init);

            _annotationItemIntegrationServiceMock = new Mock<IAnnotationItemIntegrationService>();
            _usersRepositoryMock = new Mock<IRepository<DbUser>>();
            _annotationUsersRepositoryMock = new Mock<IRepository<DbAnnotationItemAccessibleUsers>>();
            _annotationGroupsRepositoryMock = new Mock<IRepository<DbAnnotationItemAccessibleGroups>>();
            _annotationItemRepositoryMock = new Mock<IRepository<DbAnnotationItem>>();
            _usersVocabularyRepositoryMock = new Mock<IRepository<DbVocabularyUserValue>>();
            _configurationProviderMock = new Mock<IConfigurationProvider>();
            _vocabularyServiceMock = new Mock<IVocabularyService>();
            _userFilesServiceMock = new Mock<ISecurityService>();

        }

        [TestMethod]
        public void ExportTest()
        {

            _annotationUsersRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbAnnotationItemAccessibleUsers, bool>>>()))
            .Returns<Expression<Func<DbAnnotationItemAccessibleUsers, bool>>>(expr =>
            {
                return conversationStore.Where(expr.Compile());
            });

            var importExportService = new ArchiveUserDataIntegrationService(
                    _annotationItemIntegrationServiceMock.Object,
                    _usersRepositoryMock.Object,
                    _annotationUsersRepositoryMock.Object,
                    _annotationGroupsRepositoryMock.Object,
                    _annotationItemRepositoryMock.Object,
                    _usersVocabularyRepositoryMock.Object,
                    _configurationProviderMock.Object,
                    _vocabularyServiceMock.Object,
                    _userFilesServiceMock.Object);

            User user = new User();

            var data = importExportService.ExportData(user);

            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

        [TestMethod]
        public void ImportTest()
        {

            var importExportService = new ArchiveUserDataIntegrationService(
                    _annotationItemIntegrationServiceMock.Object,
                    _usersRepositoryMock.Object,
                    _annotationUsersRepositoryMock.Object,
                    _annotationGroupsRepositoryMock.Object,
                    _annotationItemRepositoryMock.Object,
                    _usersVocabularyRepositoryMock.Object,
                    _configurationProviderMock.Object,
                    _vocabularyServiceMock.Object,
                    _userFilesServiceMock.Object);

            User user = new User();

            var data = importExportService.ExportData(user);
            importExportService.ImportData(user, data);

            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

    }
}

/*

IAnnotationItemIntegrationService annotationItemIntegrationService,
IRepository<DbUser> usersRepository, 
IRepository<DbAnnotationItemAccessibleUsers> annotationUsersRepository,
IRepository<DbAnnotationItemAccessibleGroups> annotationGroupsRepository, 
IRepository<DbAnnotationItem> annotationItemRepository,
IRepository<DbVocabularyUserValue> usersVocabularyRepository, 
IConfigurationProvider configurationProvider, 
IVocabularyService vocabularyService, 
ISecurityService userFilesService

 */
