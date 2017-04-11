using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Services.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace CAFE.Tests.UnitTests.Security
{
    //[TestClass()]
    //public class SecurityServiceTests
    //{
    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        Mapper.Initialize(Services.Mapping.MapperConfig.Init);
    //    }

    //    [TestMethod]
    //    public void GetAllUsersTest()
    //    {
    //        //Arrange
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(new List<DbUser>()
    //        {
    //            new DbUser(),
    //            new DbUser(),
    //            new DbUser()
    //        }.AsQueryable());
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var users = securityService.GetAllUsers();

    //        //Assert
    //        Assert.AreEqual(users.Count(), 3);
    //    }

    //    [TestMethod]
    //    public async Task GetAllUsersAsyncTest()
    //    {
    //        //Arrange
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(new List<DbUser>()
    //        {
    //            new DbUser(),
    //            new DbUser(),
    //            new DbUser()
    //        }.AsQueryable());
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var users = await securityService.GetAllUsersAsync();

    //        //Assert
    //        Assert.AreEqual(users.Count(), 3);
    //    }

    //    [TestMethod]
    //    public void GetUserByIdWithFoundTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {Id = guid1, UserName = "User 1"},
    //            new DbUser {Id = guid2, UserName = "User 2"},
    //            new DbUser {Id = guid3, UserName = "User 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);


    //        //Act
    //        var user = securityService.GetUserById(guid1.ToString());

    //        //Assert
    //        Assert.IsNotNull(user);
    //        Assert.AreEqual(user.UserName, "User 1");
    //    }

    //    [TestMethod]
    //    public void GetUserByIdWithNotFoundTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var guid4 = Guid.NewGuid();
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {Id = guid1, UserName = "User 1"},
    //            new DbUser {Id = guid2, UserName = "User 2"},
    //            new DbUser {Id = guid3, UserName = "User 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = securityService.GetUserById(guid4.ToString());

    //        //Assert
    //        Assert.IsNull(user);
    //    }

    //    [TestMethod]
    //    public async Task GetUserByIdWithFoundAsyncTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {Id = guid1, UserName = "User 1"},
    //            new DbUser {Id = guid2, UserName = "User 2"},
    //            new DbUser {Id = guid3, UserName = "User 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.GetUserByIdAsync(guid1.ToString());

    //        //Assert
    //        Assert.IsNotNull(user);
    //        Assert.AreEqual(user.UserName, "User 1");
    //    }

    //    [TestMethod]
    //    public async Task GetUserByIdWithNotFoundAsyncTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var guid4 = Guid.NewGuid();
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {Id = guid1, UserName = "User 1"},
    //            new DbUser {Id = guid2, UserName = "User 2"},
    //            new DbUser {Id = guid3, UserName = "User 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.GetUserByIdAsync(guid4.ToString());

    //        //Assert
    //        Assert.IsNull(user);
    //    }

    //    [TestMethod]
    //    public void GetByEmailWithFoundTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);


    //        //Act
    //        var user = securityService.GetByEmail("user1@domain.com");

    //        //Assert
    //        Assert.IsNotNull(user);
    //        Assert.AreEqual(user.UserName, "User 1");
    //    }

    //    [TestMethod]
    //    public void GetByEmailWithNotFoundTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);


    //        //Act
    //        var user = securityService.GetByEmail("user6@domain.com");

    //        //Assert
    //        Assert.IsNull(user);
    //    }

    //    [TestMethod]
    //    public async Task GetByEmailWithFoundAsyncTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.GetByEmailAsync("user1@domain.com");

    //        //Assert
    //        Assert.IsNotNull(user);
    //        Assert.AreEqual(user.UserName, "User 1");
    //    }

    //    [TestMethod]
    //    public async Task GetByEmailWithNotFoundAsyncTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.GetByEmailAsync("user6@domain.com");

    //        //Assert
    //        Assert.IsNull(user);
    //    }

    //    [TestMethod]
    //    public void GetByUserNameWithFoundTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = securityService.GetByUserName("User 1");

    //        //Assert
    //        Assert.IsNotNull(user);
    //        Assert.AreEqual(user.Email, "user1@domain.com");
    //    }

    //    [TestMethod]
    //    public void GetByUserNameWithNotFoundTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = securityService.GetByUserName("User 4");

    //        //Assert
    //        Assert.IsNull(user);
    //    }

    //    [TestMethod]
    //    public async Task GetByUserNameWithFoundAsyncTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.GetByUserNameAsync("User 1");

    //        //Assert
    //        Assert.IsNotNull(user);
    //        Assert.AreEqual(user.Email, "user1@domain.com");
    //    }

    //    [TestMethod]
    //    public async Task GetByUserNameWithNotFoundAsyncTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User 1", Email = "user1@domain.com"},
    //            new DbUser {UserName = "User 2", Email = "user2@domain.com"},
    //            new DbUser {UserName = "User 3", Email = "user3@domain.com"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.FirstOrDefault(expr.Compile());
    //            });

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.GetByUserNameAsync("User 5");

    //        //Assert
    //        Assert.IsNull(user);
    //    }

    //    [TestMethod]
    //    public void CreateUserTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(new List<DbUser>().AsQueryable());

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = securityService.CreateUser();

    //        //Assert
    //        Assert.IsNotNull(user);
    //    }

    //    [TestMethod]
    //    public async Task CreateUserAsyncTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(new List<DbUser>().AsQueryable());

    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var user = await securityService.CreateUserAsync();

    //        //Assert
    //        Assert.IsNotNull(user);
    //    }

    //    [TestMethod]
    //    public void SaveUserNewTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Insert(It.IsAny<DbUser>())).Callback<DbUser>(u =>
    //        {
    //            u.Id = id;
    //            usersStore.Add(u);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var user = new User();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.UserName = "User 1";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        var savedUser = securityService.SaveUser(user);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(savedUser);
    //        Assert.IsNotNull(userInStore);
    //        Assert.AreEqual(savedUser.Id, id.ToString());
    //        Assert.AreEqual(userInStore.Id, id);
    //    }

    //    [TestMethod]
    //    public void SaveUserExistTest()
    //    {
    //        //Arrange
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Update(It.IsAny<DbUser>())).Returns<DbUser>(dbUser =>
    //        {
    //            var found = usersStore.First(u => u.Id == id);
    //            return Mapper.Map(dbUser, found);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);


    //        var dbuser = new DbUser();
    //        dbuser.Id = id;
    //        dbuser.Email = "user1@domain.com";
    //        dbuser.IsActive = true;
    //        dbuser.UserName = "User 1";
    //        dbuser.Name = "User";
    //        dbuser.Surname = "Domain";
    //        usersStore.Add(dbuser);

    //        var user = new User();
    //        user.Id = id.ToString();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.UserName = "User 11";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        var savedUser = securityService.SaveUser(user);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(savedUser);
    //        Assert.IsNotNull(userInStore);
    //        Assert.AreEqual(savedUser.UserName, "User 11");
    //        Assert.AreEqual(userInStore.UserName, "User 11");
    //    }

    //    [TestMethod]
    //    public async Task SaveUserNewAsyncTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Insert(It.IsAny<DbUser>())).Callback<DbUser>(u =>
    //        {
    //            u.Id = id;
    //            usersStore.Add(u);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var user = new User();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.UserName = "User 1";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        var savedUser = await securityService.SaveUserAsync(user);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(savedUser);
    //        Assert.IsNotNull(userInStore);
    //        Assert.AreEqual(savedUser.Id, id.ToString());
    //        Assert.AreEqual(userInStore.Id, id);
    //    }

    //    [TestMethod()]
    //    public async Task SaveUserExistAsyncTest()
    //    {
    //        //Arrange
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Update(It.IsAny<DbUser>())).Returns<DbUser>(dbUser =>
    //        {
    //            var found = usersStore.First(u => u.Id == id);
    //            return Mapper.Map(dbUser, found);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbuser = new DbUser();
    //        dbuser.Id = id;
    //        dbuser.Email = "user1@domain.com";
    //        dbuser.IsActive = true;
    //        dbuser.UserName = "User 1";
    //        dbuser.Name = "User";
    //        dbuser.Surname = "Domain";
    //        usersStore.Add(dbuser);

    //        var user = new User();
    //        user.Id = id.ToString();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.UserName = "User 11";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        var savedUser = await securityService.SaveUserAsync(user);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(savedUser);
    //        Assert.IsNotNull(userInStore);
    //        Assert.AreEqual(savedUser.UserName, "User 11");
    //        Assert.AreEqual(userInStore.UserName, "User 11");
    //    }

    //    [TestMethod]
    //    public void RemoveUserTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Update(It.IsAny<DbUser>())).Returns<DbUser>(dbUser =>
    //        {
    //            var found = usersStore.First(u => u.Id == id);
    //            return Mapper.Map(dbUser, found);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbuser = new DbUser();
    //        dbuser.Id = id;
    //        dbuser.Email = "user1@domain.com";
    //        dbuser.IsActive = true;
    //        dbuser.UserName = "User 1";
    //        dbuser.Name = "User";
    //        dbuser.Surname = "Domain";
    //        usersStore.Add(dbuser);


    //        var user = new User();
    //        user.Id = id.ToString();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.UserName = "User 1";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        securityService.RemoveUser(user);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(userInStore);
    //        Assert.IsFalse(userInStore.IsActive);
    //    }

    //    [TestMethod]
    //    public async Task RemoveUserAsyncTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Update(It.IsAny<DbUser>())).Returns<DbUser>(dbUser =>
    //        {
    //            var found = usersStore.First(u => u.Id == id);
    //            return Mapper.Map(dbUser, found);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbuser = new DbUser();
    //        dbuser.Id = id;
    //        dbuser.Email = "user1@domain.com";
    //        dbuser.IsActive = true;
    //        dbuser.UserName = "User 1";
    //        dbuser.Name = "User";
    //        dbuser.Surname = "Domain";
    //        usersStore.Add(dbuser);



    //        var user = new User();
    //        user.Id = id.ToString();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.UserName = "User 1";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        await securityService.RemoveUserAsync(user);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(userInStore);
    //        Assert.IsFalse(userInStore.IsActive);
    //    }

    //    [TestMethod]
    //    public void AcceptUserTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Update(It.IsAny<DbUser>())).Returns<DbUser>(dbUser =>
    //        {
    //            var found = usersStore.First(u => u.Id == id);
    //            return Mapper.Map(dbUser, found);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);


    //        var dbuser = new DbUser();
    //        dbuser.Id = id;
    //        dbuser.Email = "user1@domain.com";
    //        dbuser.IsActive = true;
    //        dbuser.IsAccepted = false;
    //        dbuser.UserName = "User 1";
    //        dbuser.Name = "User";
    //        dbuser.Surname = "Domain";
    //        usersStore.Add(dbuser);



    //        var user = new User();
    //        user.Id = id.ToString();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.IsAccepted = false;
    //        user.UserName = "User 1";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        securityService.AcceptUser(user.Id);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(userInStore);
    //        Assert.IsTrue(userInStore.IsAccepted);
    //    }

    //    [TestMethod]
    //    public async Task AcceptUserAsyncTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var usersStore = new List<DbUser>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.Select()).Returns(usersStore.AsQueryable());
    //        userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return usersStore.FirstOrDefault(expr.Compile());
    //            });
    //        userRepositoryMock.Setup(a => a.Update(It.IsAny<DbUser>())).Returns<DbUser>(dbUser =>
    //        {
    //            var found = usersStore.First(u => u.Id == id);
    //            return Mapper.Map(dbUser, found);
    //        });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbuser = new DbUser();
    //        dbuser.Id = id;
    //        dbuser.Email = "user1@domain.com";
    //        dbuser.IsActive = true;
    //        dbuser.IsAccepted = false;
    //        dbuser.UserName = "User 1";
    //        dbuser.Name = "User";
    //        dbuser.Surname = "Domain";
    //        usersStore.Add(dbuser);


    //        var user = new User();
    //        user.Id = id.ToString();
    //        user.Email = "user1@domain.com";
    //        user.IsActive = true;
    //        user.IsAccepted = false;
    //        user.UserName = "User 1";
    //        user.Name = "User";
    //        user.Surname = "Domain";

    //        //Act
    //        await securityService.AcceptUserAsync(user.Id);
    //        var userInStore = usersStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(userInStore);
    //        Assert.IsTrue(userInStore.IsAccepted);
    //    }

    //    [TestMethod]
    //    public void SearchUsersTest()
    //    {
    //        //Arrange
    //        var userStore = new List<DbUser>()
    //        {
    //            new DbUser {UserName = "User1", Name = "User 1", Surname = "Domain 1"},
    //            new DbUser {UserName = "User2", Name = "User 2", Surname = "Domain 2"},
    //            new DbUser {UserName = "User3", Name = "User 3", Surname = "Domain 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        userRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbUser, bool>>>()))
    //            .Returns<Expression<Func<DbUser, bool>>>(expr =>
    //            {
    //                return userStore.Where(expr.Compile());
    //            });
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var foundUsersByUserName = securityService.SearchUsers("user1").ToList();
    //        var foundAllUsersByName = securityService.SearchUsers("user").ToList();
    //        var foundUsersBySurname = securityService.SearchUsers("domain 3").ToList();
    //        var notFoundUsersBySurname = securityService.SearchUsers("domain 5").ToList();

    //        //Assert
    //        Assert.IsNotNull(foundUsersByUserName);
    //        Assert.IsNotNull(foundAllUsersByName);
    //        Assert.IsNotNull(foundUsersBySurname);
    //        Assert.IsNotNull(notFoundUsersBySurname);

    //        Assert.AreEqual(foundUsersByUserName.Count, 1);
    //        Assert.AreEqual(foundAllUsersByName.Count, 3);
    //        Assert.AreEqual(foundUsersBySurname.Count, 1);
    //        Assert.AreEqual(notFoundUsersBySurname.Count, 0);

    //        Assert.IsTrue(foundUsersByUserName.Any(a => a.UserName == "User1"));
    //        Assert.IsTrue(foundAllUsersByName.Any(a => a.UserName == "User1"));
    //        Assert.IsTrue(foundAllUsersByName.Any(a => a.UserName == "User3"));

    //    }

    //    [TestMethod]
    //    public void GetAllGroupsTest()
    //    {
    //        //Arrange
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(new List<DbRole>()
    //        {
    //                new DbRole {IsGroup = true},
    //                new DbRole {IsGroup = true},
    //                new DbRole {IsGroup = true}
    //        }.AsQueryable());

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var groups = securityService.GetAllGroups();

    //        //Assert
    //        Assert.AreEqual(groups.Count(), 3);
    //    }

    //    [TestMethod]
    //    public async Task GetAllGroupsAsyncTest()
    //    {
    //        //Arrange
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(new List<DbRole>()
    //        {
    //                new DbRole {IsGroup = true},
    //                new DbRole {IsGroup = true},
    //                new DbRole {IsGroup = true}
    //        }.AsQueryable());

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var groups = await securityService.GetAllGroupsAsync();

    //        //Assert
    //        Assert.AreEqual(groups.Count(), 3);
    //    }

    //    [TestMethod]
    //    public void GetGroupByIdWithFoundTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var groupStore = new List<DbRole>()
    //        {
    //            new DbRole {Id = guid1, Name = "Group 1"},
    //            new DbRole {Id = guid2, Name = "Group 2"},
    //            new DbRole {Id = guid3, Name = "Group 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var group = securityService.GetGroupById(guid1.ToString());

    //        //Assert
    //        Assert.IsNotNull(group);
    //        Assert.AreEqual(group.Name, "Group 1");
    //    }

    //    [TestMethod]
    //    public void GetGroupByIdWithNotFoundTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var guid4 = Guid.NewGuid();
    //        var groupStore = new List<DbRole>()
    //        {
    //            new DbRole {Id = guid1, Name = "Group 1"},
    //            new DbRole {Id = guid2, Name = "Group 2"},
    //            new DbRole {Id = guid3, Name = "Group 3"}
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var group = securityService.GetGroupById(guid4.ToString());

    //        //Assert
    //        Assert.IsNull(group);
    //    }

    //    [TestMethod]
    //    public async Task GetGroupByIdWithFoundAsyncTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var groupStore = new List<DbRole>()
    //        {
    //            new DbRole {Id = guid1, Name = "Group 1", IsGroup = true },
    //            new DbRole {Id = guid2, Name = "Group 2", IsGroup = true },
    //            new DbRole {Id = guid3, Name = "Group 3", IsGroup = true }
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var group = await securityService.GetGroupByIdAsync(guid1.ToString());

    //        //Assert
    //        Assert.IsNotNull(group);
    //        Assert.AreEqual(group.Name, "Group 1");
    //    }

    //    [TestMethod]
    //    public async Task GetGroupByIdWithNotFoundAsyncTest()
    //    {
    //        //Arrange
    //        var guid1 = Guid.NewGuid();
    //        var guid2 = Guid.NewGuid();
    //        var guid3 = Guid.NewGuid();
    //        var guid4 = Guid.NewGuid();
    //        var groupStore = new List<DbRole>()
    //        {
    //            new DbRole {Id = guid1, Name = "Group 1", IsGroup = true },
    //            new DbRole {Id = guid2, Name = "Group 2", IsGroup = true },
    //            new DbRole {Id = guid3, Name = "Group 3", IsGroup = true }
    //        };

    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var group = await securityService.GetGroupByIdAsync(guid4.ToString());

    //        //Assert
    //        Assert.IsNull(group);
    //    }

    //    [TestMethod]
    //    public void CreateGroupTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var group = securityService.CreateGroup();

    //        //Assert
    //        Assert.IsNotNull(group);
    //    }

    //    [TestMethod]
    //    public async Task CreateGroupAsyncTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(new List<DbRole>().AsQueryable());

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var group = await securityService.CreateGroupAsync();

    //        //Assert
    //        Assert.IsNotNull(group);
    //    }

    //    [TestMethod]
    //    public void AddGroupTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var groupStore = new List<DbRole>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });
    //        roleRepositoryMock.Setup(a => a.Insert(It.IsAny<DbRole>())).Callback<DbRole>(r =>
    //        {
    //            r.Id = id;
    //            groupStore.Add(r);
    //        });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var group = new Group();
    //        group.Name = "Group 1";
    //        group.IsGroup = true;

    //        //Act
    //        var addedGroup = securityService.AddGroup(group);
    //        var groupInStore = groupStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(addedGroup);
    //        Assert.IsNotNull(groupInStore);
    //        Assert.AreEqual(addedGroup.Id, id.ToString());
    //        Assert.AreEqual(groupInStore.Id, id);
    //    }

    //    [TestMethod]
    //    public async Task AddGroupAsyncTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var groupStore = new List<DbRole>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });
    //        roleRepositoryMock.Setup(a => a.Insert(It.IsAny<DbRole>())).Callback<DbRole>(r =>
    //        {
    //            r.Id = id;
    //            groupStore.Add(r);
    //        });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var group = new Group();
    //        group.Name = "Group 1";
    //        group.IsGroup = true;

    //        //Act
    //        var addedGroup = await securityService.AddGroupAsync(group);
    //        var groupInStore = groupStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsNotNull(addedGroup);
    //        Assert.IsNotNull(groupInStore);
    //        Assert.AreEqual(addedGroup.Id, id.ToString());
    //        Assert.AreEqual(groupInStore.Id, id);
    //    }

    //    [TestMethod]
    //    public void RemoveGroupTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var groupStore = new List<DbRole>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });
    //        roleRepositoryMock.Setup(a => a.Delete(It.IsAny<DbRole>())).Callback<DbRole>(r =>
    //        {
    //            groupStore.Remove(r);
    //        });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbRole = new DbRole
    //        {
    //            Id = id,
    //            Name = "Group 1",
    //            IsGroup = true
    //        };
    //        groupStore.Add(dbRole);


    //        var group = new Group();
    //        group.Id = id.ToString();
    //        group.Name = "Group 1";
    //        group.IsGroup = true;


    //        //Act
    //        var removeResult = securityService.RemoveGroup(group);
    //        var groupInStore = groupStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsTrue(removeResult);
    //        Assert.IsNull(groupInStore);
    //    }

    //    [TestMethod]
    //    public async Task RemoveGroupAsyncTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var groupStore = new List<DbRole>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });
    //        roleRepositoryMock.Setup(a => a.Delete(It.IsAny<DbRole>())).Callback<DbRole>(r =>
    //        {
    //            groupStore.Remove(r);
    //        });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbRole = new DbRole
    //        {
    //            Id = id,
    //            Name = "Group 1",
    //            IsGroup = true
    //        };
    //        groupStore.Add(dbRole);


    //        var group = new Group();
    //        group.Id = id.ToString();
    //        group.Name = "Group 1";
    //        group.IsGroup = true;


    //        //Act
    //        var removeResult = await securityService.RemoveGroupAsync(group);
    //        var groupInStore = groupStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsTrue(removeResult);
    //        Assert.IsNull(groupInStore);
    //    }

    //    [TestMethod]
    //    public void UpdateGroupTest()
    //    {
    //        //Arrange         
    //        var id = Guid.NewGuid();
    //        var groupStore = new List<DbRole>();
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        roleRepositoryMock.Setup(a => a.Select()).Returns(groupStore.AsQueryable());
    //        roleRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbRole, bool>>>()))
    //            .Returns<Expression<Func<DbRole, bool>>>(expr =>
    //            {
    //                return groupStore.FirstOrDefault(expr.Compile());
    //            });
    //        roleRepositoryMock.Setup(a => a.Update(It.IsAny<DbRole>())).Returns<DbRole>(role =>
    //        {
    //            var found = groupStore.First(u => u.Id == id);
    //            return Mapper.Map(role, found);
    //        });

    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        var dbRole = new DbRole
    //        {
    //            Id = id,
    //            Name = "Group 1",
    //            IsGroup = true
    //        };
    //        groupStore.Add(dbRole);


    //        var group = new Group();
    //        group.Id = id.ToString();
    //        group.Name = "Group 11";
    //        group.IsGroup = true;


    //        //Act
    //        var updatedGroup = securityService.UpdateGroup(group.Id, group.Name, new List<string>(), new List<string>());
    //        var groupInStore = groupStore.FirstOrDefault(u => u.Id == id);

    //        //Assert
    //        Assert.IsTrue(updatedGroup);
    //        Assert.IsNotNull(groupInStore);
    //        Assert.AreEqual(groupInStore.Name, "Group 11");
    //    }

    //    [TestMethod]
    //    public void AddGroupUserTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var result = securityService.AddGroupUser("");

    //        //Assert
    //        Assert.IsTrue(result);
    //    }

    //    [TestMethod]
    //    public async Task AddGroupUserAsyncTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var result = await securityService.AddGroupUserAsync("");

    //        //Assert
    //        Assert.IsTrue(result);
    //    }

    //    [TestMethod]
    //    public void RemoveGroupUserTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var result = securityService.RemoveGroupUser("");

    //        //Assert
    //        Assert.IsTrue(result);
    //    }

    //    [TestMethod]
    //    public async Task RemoveGroupUserAsyncTest()
    //    {
    //        //Arrange         
    //        var userRepositoryMock = new Mock<IRepository<DbUser>>();
    //        var roleRepositoryMock = new Mock<IRepository<DbRole>>();
    //        var userFileRepositoryMock = new Mock<IRepository<DbUserFile>>();
    //        var securityService = new SecurityService(userRepositoryMock.Object, roleRepositoryMock.Object, userFileRepositoryMock.Object);

    //        //Act
    //        var result = await securityService.RemoveGroupUserAsync("");

    //        //Assert
    //        Assert.IsTrue(result);
    //    }

    //}
}