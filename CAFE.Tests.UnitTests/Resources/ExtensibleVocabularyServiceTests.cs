using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CAFE.Core.Integration;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.DAL.Models.Resources;
using CAFE.Services.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CAFE.Tests.UnitTests.Resources
{
    [TestClass()]
    public class ExtensibleVocabularyServiceTests
    {
        Mock<IRepository<DbUser>> _usersRepositoryMock;
        Mock<IRepository<DbVocabularyValue>> _vocabularyRepositoryMock;
        Mock<IRepository<DbVocabularyUserValue>> _vocabularyUsersRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            Mapper.Initialize(Services.Mapping.MapperConfig.Init);

            _usersRepositoryMock = new Mock<IRepository<DbUser>>();
            _vocabularyRepositoryMock = new Mock<IRepository<DbVocabularyValue>>();
            _vocabularyUsersRepositoryMock = new Mock<IRepository<DbVocabularyUserValue>>();
        }

        [TestMethod()]
        public void ExtensibleVocabularyServiceTest()
        {
            //Arrange
            //Act
            var service = InitService();

            //Assert
            Assert.IsNotNull(service);
        }

        private ExtensibleVocabularyService InitService()
        {
            return new ExtensibleVocabularyService(
                _vocabularyRepositoryMock.Object,
                _vocabularyUsersRepositoryMock.Object,
                _usersRepositoryMock.Object);
        }

        [TestMethod()]
        public void GetAllExtenededValuesTest()
        {
            //Arrange
            var vocabulariesStore = new List<DbVocabularyValue>();
            vocabulariesStore.Add(new DbVocabularyValue() {Id = 1, Type = "TimezoneVocabulary", Value = "TimeZone1"});
            vocabulariesStore.Add(new DbVocabularyValue() { Id = 2, Type = "TimezoneVocabulary", Value = "TimeZone2" });

            _vocabularyRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyValue, bool>>>(expr =>
                {
                    return vocabulariesStore.Where(expr.Compile());
                });

            var service = InitService();

            //Act
            var vocabularyValues = service.GetAllExtenededValues(typeof(TimezoneVocabulary)).ToList();

            //Assert
            Assert.IsNotNull(vocabularyValues);
            Assert.IsTrue(vocabularyValues.Count > 0);
            Assert.IsTrue(vocabularyValues.Any(a => a == "TimeZone1"));
        }

        [TestMethod()]
        public void GetAllExtenededValuesTestGeneric()
        {
            //Arrange
            var vocabulariesStore = new List<DbVocabularyValue>();
            vocabulariesStore.Add(new DbVocabularyValue() { Id = 1, Type = "TimezoneVocabulary", Value = "TimeZone1" });
            vocabulariesStore.Add(new DbVocabularyValue() { Id = 2, Type = "TimezoneVocabulary", Value = "TimeZone2" });

            _vocabularyRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyValue, bool>>>(expr =>
                {
                    return vocabulariesStore.Where(expr.Compile());
                });

            var service = InitService();

            //Act
            var vocabularyValues = service.GetAllExtenededValues<TimezoneVocabulary>().ToList();

            //Assert
            Assert.IsNotNull(vocabularyValues);
            Assert.IsTrue(vocabularyValues.Count > 0);
            Assert.IsTrue(vocabularyValues.Any(a => a == "TimeZone1"));
        }

        [TestMethod()]
        public void GetExtenededValuesByTest()
        {
            //Arrange
            var dbUser1 = new DbUser()
            {
                Id = Guid.NewGuid()
            };
            var dbUser2 = new DbUser()
            {
                Id = Guid.NewGuid()
            };
            var vocabulariesStore = new List<DbVocabularyUserValue>();
            vocabulariesStore.Add(new DbVocabularyUserValue() { Id = 1, User = dbUser1, Type = "TimezoneVocabulary", Value = "TimeZone1" });
            vocabulariesStore.Add(new DbVocabularyUserValue() { Id = 2, User = dbUser2, Type = "TimezoneVocabulary", Value = "TimeZone2" });

            _vocabularyUsersRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyUserValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyUserValue, bool>>>(expr =>
                {
                    return vocabulariesStore.Where(expr.Compile());
                });

            var service = InitService();

            //Act
            var vocabularyValues = service.GetExtenededValuesBy(dbUser1.Id.ToString(), typeof(TimezoneVocabulary)).ToList();

            //Assert
            Assert.IsNotNull(vocabularyValues);
            Assert.IsTrue(vocabularyValues.Count == 1);
            Assert.IsTrue(vocabularyValues.Any(a => a == "TimeZone1"));
            Assert.IsTrue(vocabularyValues.All(a => a != "TimeZone2"));
        }

        [TestMethod()]
        public void GetExtenededValuesByTestGeneric()
        {
            //Arrange
            var dbUser1 = new DbUser()
            {
                Id = Guid.NewGuid()
            };
            var dbUser2 = new DbUser()
            {
                Id = Guid.NewGuid()
            };
            var vocabulariesStore = new List<DbVocabularyUserValue>();
            vocabulariesStore.Add(new DbVocabularyUserValue() { Id = 1, User = dbUser1, Type = "TimezoneVocabulary", Value = "TimeZone1" });
            vocabulariesStore.Add(new DbVocabularyUserValue() { Id = 2, User = dbUser2, Type = "TimezoneVocabulary", Value = "TimeZone2" });

            _vocabularyUsersRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyUserValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyUserValue, bool>>>(expr =>
                {
                    return vocabulariesStore.Where(expr.Compile());
                });

            var service = InitService();

            //Act
            var vocabularyValues = service.GetExtenededValuesBy<TimezoneVocabulary>(dbUser1.Id.ToString()).ToList();

            //Assert
            Assert.IsNotNull(vocabularyValues);
            Assert.IsTrue(vocabularyValues.Count == 1);
            Assert.IsTrue(vocabularyValues.Any(a => a == "TimeZone1"));
            Assert.IsTrue(vocabularyValues.All(a => a != "TimeZone2"));
        }

        [TestMethod()]
        public void AddNewExtendedValuesByTest()
        {
            //Arrange
            var dbUser1 = new DbUser()
            {
                Id = Guid.NewGuid()
            };
            var vocabulariesStore = new List<DbVocabularyUserValue>();
            var ids = 1;
            _vocabularyUsersRepositoryMock.Setup(s => s.Insert(It.IsAny<DbVocabularyUserValue>())).Callback<DbVocabularyUserValue>(e =>
            {
                e.Id = ids++;
                vocabulariesStore.Add(e);
            });
            _vocabularyUsersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbVocabularyUserValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyUserValue, bool>>>(expr =>
                {
                    return vocabulariesStore.FirstOrDefault(expr.Compile());
                });

            _usersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Returns<Expression<Func<DbUser, bool>>>(expr =>
                {
                    return dbUser1;
                });

            var service = InitService();

            //Act
            service.AddNewExtendedValuesBy(dbUser1.Id.ToString(), typeof (TimezoneVocabulary),
                new List<string> {"TimeZone1", "TimeZone2"});

            //Assert
            Assert.IsTrue(vocabulariesStore.Count == 2);
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone1"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone2"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Id == 1));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Id == 2));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Type == "TimezoneVocabulary"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.User.Id == dbUser1.Id));
        }

        [TestMethod()]
        public void AddNewExtendedValuesByTestGeneric()
        {
            //Arrange
            var dbUser1 = new DbUser()
            {
                Id = Guid.NewGuid()
            };
            var vocabulariesStore = new List<DbVocabularyUserValue>();
            var ids = 1;
            _vocabularyUsersRepositoryMock.Setup(s => s.Insert(It.IsAny<DbVocabularyUserValue>())).Callback<DbVocabularyUserValue>(e =>
            {
                e.Id = ids++;
                vocabulariesStore.Add(e);
            });
            _vocabularyUsersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbVocabularyUserValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyUserValue, bool>>>(expr =>
                {
                    return vocabulariesStore.FirstOrDefault(expr.Compile());
                });
            _usersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Returns<Expression<Func<DbUser, bool>>>(expr =>
                {
                    return dbUser1;
                });

            var service = InitService();

            //Act
            service.AddNewExtendedValuesBy<TimezoneVocabulary>(dbUser1.Id.ToString(), 
                new List<string> { "TimeZone1", "TimeZone2" });

            //Assert
            Assert.IsTrue(vocabulariesStore.Count == 2);
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone1"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone2"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Id == 1));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Id == 2));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Type == "TimezoneVocabulary"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.User.Id == dbUser1.Id));
        }

        [TestMethod()]
        public void MakeExtenededValuesGlobalyTest()
        {
            //Arrange
            var dbUser1 = new DbUser()
            {
                Id = Guid.NewGuid()
            };

            var vocabulariesUsersStore = new List<DbVocabularyUserValue>();
            vocabulariesUsersStore.Add(new DbVocabularyUserValue() { Id = 1, User = dbUser1, Type = "TimezoneVocabulary", Value = "TimeZone1" });
            vocabulariesUsersStore.Add(new DbVocabularyUserValue() { Id = 2, User = dbUser1, Type = "TimezoneVocabulary", Value = "TimeZone2" });

            _vocabularyUsersRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyUserValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyUserValue, bool>>>(expr =>
                {
                    return vocabulariesUsersStore.Where(expr.Compile());
                });
            _vocabularyUsersRepositoryMock.Setup(a => a.Delete(It.IsAny<DbVocabularyUserValue>()))
                .Callback<DbVocabularyUserValue>(
                    e =>
                    {
                        vocabulariesUsersStore.Remove(e);
                    });

            var vocabulariesStore = new List<DbVocabularyValue>();
            _vocabularyRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbVocabularyValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyValue, bool>>>(expr =>
                {
                    return vocabulariesStore.FirstOrDefault(expr.Compile());
                });
            _vocabularyRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyValue, bool>>>(expr =>
                {
                    return vocabulariesStore.Where(expr.Compile());
                });

            var ids = 1;
            _vocabularyRepositoryMock.Setup(s => s.Insert(It.IsAny<DbVocabularyValue>())).Callback<DbVocabularyValue>(e =>
            {
                e.Id = ids++;
                vocabulariesStore.Add(e);
            });
            var service = InitService();

            //Act
            var globalVocabularyValues =
                service.MakeExtenededValuesGlobaly(dbUser1.Id.ToString(), typeof (TimezoneVocabulary),
                    new List<string> {"TimeZone1", "TimeZone2"}).ToList();

    
            //Assert
            Assert.IsNotNull(globalVocabularyValues);
            Assert.IsTrue(globalVocabularyValues.Count == 2);
            Assert.IsTrue(vocabulariesStore.Count == 2);
            Assert.IsTrue(vocabulariesUsersStore.Count == 0);
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone1"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone2"));
        }

        [TestMethod()]
        public void MakeExtenededValuesGlobalyTestGeneric()
        {
            //Arrange
            var dbUser1 = new DbUser()
            {
                Id = Guid.NewGuid()
            };

            var vocabulariesUsersStore = new List<DbVocabularyUserValue>();
            vocabulariesUsersStore.Add(new DbVocabularyUserValue() { Id = 1, User = dbUser1, Type = "TimezoneVocabulary", Value = "TimeZone1" });
            vocabulariesUsersStore.Add(new DbVocabularyUserValue() { Id = 2, User = dbUser1, Type = "TimezoneVocabulary", Value = "TimeZone2" });

            _vocabularyUsersRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyUserValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyUserValue, bool>>>(expr =>
                {
                    return vocabulariesUsersStore.Where(expr.Compile());
                });
            _vocabularyUsersRepositoryMock.Setup(a => a.Delete(It.IsAny<DbVocabularyUserValue>()))
                .Callback<DbVocabularyUserValue>(
                    e =>
                    {
                        vocabulariesUsersStore.Remove(e);
                    });

            var vocabulariesStore = new List<DbVocabularyValue>();
            _vocabularyRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbVocabularyValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyValue, bool>>>(expr =>
                {
                    return vocabulariesStore.FirstOrDefault(expr.Compile());
                });
            _vocabularyRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbVocabularyValue, bool>>>()))
                .Returns<Expression<Func<DbVocabularyValue, bool>>>(expr =>
                {
                    return vocabulariesStore.Where(expr.Compile());
                });

            var ids = 1;
            _vocabularyRepositoryMock.Setup(s => s.Insert(It.IsAny<DbVocabularyValue>())).Callback<DbVocabularyValue>(e =>
            {
                e.Id = ids++;
                vocabulariesStore.Add(e);
            });
            var service = InitService();

            //Act
            var globalVocabularyValues =
                service.MakeExtenededValuesGlobaly<TimezoneVocabulary>(dbUser1.Id.ToString(),
                    new List<string> { "TimeZone1", "TimeZone2" }).ToList();


            //Assert
            Assert.IsNotNull(globalVocabularyValues);
            Assert.IsTrue(globalVocabularyValues.Count == 2);
            Assert.IsTrue(vocabulariesStore.Count == 2);
            Assert.IsTrue(vocabulariesUsersStore.Count == 0);
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone1"));
            Assert.IsTrue(vocabulariesStore.Any(a => a.Value == "TimeZone2"));
        }
    }
}