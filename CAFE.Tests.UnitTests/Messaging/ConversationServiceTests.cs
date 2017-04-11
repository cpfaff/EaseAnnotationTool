using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Services.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DateTime = System.DateTime;

namespace CAFE.Tests.UnitTests.Messaging
{
    [TestClass()]
    public class ConversationServiceTests
    {
        Mock<IRepository<DbAccessRequest>> _accessRequestRepositoryMock;
        Mock<IRepository<DbConversation>> _conversationRepositoryMock;
        Mock<IRepository<DbUser>> _usersRepositoryMock;
        Mock<IRepository<DbMessage>> _messageRepositoryMock;
        Mock<IRepository<DbUserFile>> _filesRepositoryMock;
        Mock<IRepository<DbAnnotationItem>> _annotationItemRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            Mapper.Initialize(Services.Mapping.MapperConfig.Init);

            _accessRequestRepositoryMock = new Mock<IRepository<DbAccessRequest>>();
            _conversationRepositoryMock = new Mock<IRepository<DbConversation>>();
            _usersRepositoryMock = new Mock<IRepository<DbUser>>();
            _messageRepositoryMock = new Mock<IRepository<DbMessage>>();
            _filesRepositoryMock = new Mock<IRepository<DbUserFile>>();
            _annotationItemRepositoryMock = new Mock<IRepository<DbAnnotationItem>>();
        }

        [TestMethod()]
        public void ConversationServiceTest()
        {
            //Arrange
            //Act
            var service = InitService();

            //Assert
            Assert.IsNotNull(service);
        }

        [TestMethod, ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ConversationServiceTest_ConstructorFail()
        {
            //Arrange
            //Act
            var service = InitServiceWithouConstructorParams();

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void OpenConversationForTest()
        {
            //Arrange
            var accessRequestStore = new List<DbAccessRequest>();
            accessRequestStore.Add(new DbAccessRequest() {Id = 1});

            _accessRequestRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbAccessRequest, bool>>>()))
                .Returns<Expression<Func<DbAccessRequest, bool>>>(expr =>
                {
                    return accessRequestStore.FirstOrDefault(expr.Compile());
                });

            var userStore = new List<DbUser>();
            var user1Guid = Guid.NewGuid();
            var user2Guid = Guid.NewGuid();
            userStore.Add(new DbUser() { Id = user1Guid });
            userStore.Add(new DbUser() { Id = user2Guid });

            _usersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Returns<Expression<Func<DbUser, bool>>>(expr =>
                {
                    return userStore.FirstOrDefault(expr.Compile());
                });

            var conversationStore = new List<DbConversation>();
            var newConversationId = 1;
            _conversationRepositoryMock.Setup(s => s.Insert(It.IsAny<DbConversation>())).Callback<DbConversation>(e =>
            {
                e.Id = newConversationId;
                conversationStore.Add(e);
            });

            var messageStore = new List<DbMessage>();
            var newMessageId = 1;
            _messageRepositoryMock.Setup(s => s.Insert(It.IsAny<DbMessage>())).Callback<DbMessage>(e =>
            {
                e.Id = newMessageId;
                messageStore.Add(e);
            });

            var service = InitService();
            var subjectText = "Test request subject";
            var messageText = "Test request message";

            //Act
            var conversation = service.OpenConversationFor(
                accessRequestId: 1,
                requesterId: user1Guid.ToString(),
                receiverId: user2Guid.ToString(),
                subject: subjectText,
                message: messageText);

            var message = messageStore.FirstOrDefault(f => f.Id == newMessageId);

            //Assert
            Assert.IsNotNull(conversation);
            Assert.AreEqual(conversation.Id, newConversationId);
            Assert.IsTrue(conversationStore.Any(a => a.Id == newConversationId));
            Assert.IsNotNull(message);
            Assert.AreEqual(message.Conversation.Id, newConversationId);
            Assert.AreEqual(conversation.Request.Id, 1);
            Assert.AreEqual(conversation.Requester.Id, user1Guid.ToString());
            Assert.AreEqual(conversation.Receiver.Id, user2Guid.ToString());
            Assert.AreEqual(message.Sender.Id, user1Guid);
            Assert.AreEqual(message.Receiver.Id, user2Guid);
            Assert.AreEqual(message.Text, string.Concat(subjectText, "\n\r", messageText));
            Assert.AreEqual(conversation.Status, AccessRequestStatus.Open);
            //Assert.IsTrue(conversation.HasRecieverUnreadMessages);
        }

        [TestMethod()]
        public void GetConversationsForTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() {Id = 1};
            var user1 = new DbUser() { Id = Guid.NewGuid() };
            var user2 = new DbUser() { Id = Guid.NewGuid() };

            var subjectText = "Test request subject";
            var messageText = "Test request message";

            var conversationStore = new List<DbConversation>();
            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = user2,
                Requester = user1,
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };
            _conversationRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversationStore.Where(expr.Compile());
                });

            var messages = new List<DbMessage>();
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user2,
                Sender = user1,
                Text = string.Concat(subjectText, "\n\r", messageText)
            });
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user1,
                Sender = user2,
                Text = string.Concat("RE: ", subjectText, "\n\r",messageText)
            });
            conversation.Messages = messages;
            conversationStore.Add(conversation);

            var service = InitService();

            //Act
            var conversations = service.GetConversationsFor(accessRequest.Id).ToList();
            
            //Assert
            Assert.IsNotNull(conversations);
            Assert.IsTrue(conversations.Count > 0);
            Assert.IsTrue(conversations.Any(a => a.Id == 1));
            Assert.IsTrue(conversations.All(a => a.Request != null));
            Assert.IsTrue(conversations.All(a => a.Requester != null));
            Assert.IsTrue(conversations.All(a => a.Receiver != null));
            Assert.IsTrue(conversations.Any(a => a.Messages.Count() > 0));
        }

        [TestMethod()]
        public void GetConversationsBySenderTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };
            var user1 = new DbUser() { Id = Guid.NewGuid() };
            var user2 = new DbUser() { Id = Guid.NewGuid() };

            var subjectText = "Test request subject";
            var messageText = "Test request message";

            var conversationStore = new List<DbConversation>();
            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = user2,
                Requester = user1,
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };
            _conversationRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversationStore.Where(expr.Compile());
                });

            var messages = new List<DbMessage>();
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user2,
                Sender = user1,
                Text = string.Concat(subjectText, "\n\r", messageText)
            });
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user1,
                Sender = user2,
                Text = string.Concat("RE: ", subjectText, "\n\r", messageText)
            });
            conversation.Messages = messages;
            conversationStore.Add(conversation);

            var service = InitService();

            //Act
            var conversations = service.GetConversationsBySender(Mapper.Map<User>(user1)).ToList();

            //Assert
            Assert.IsNotNull(conversations);
            Assert.IsTrue(conversations.Count > 0);
            Assert.IsTrue(conversations.Any(a => a.Id == 1));
            Assert.IsTrue(conversations.All(a => a.Request != null));
            Assert.IsTrue(conversations.All(a => a.Requester != null));
            Assert.IsTrue(conversations.All(a => a.Receiver != null));
            Assert.IsTrue(conversations.Any(a => a.Messages.Count() > 0));
        }

        [TestMethod()]
        public void GetConversationsByReceiverTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };
            var user1 = new DbUser() { Id = Guid.NewGuid() };
            var user2 = new DbUser() { Id = Guid.NewGuid() };

            var subjectText = "Test request subject";
            var messageText = "Test request message";

            var conversationStore = new List<DbConversation>();
            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = user2,
                Requester = user1,
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };
            _conversationRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversationStore.Where(expr.Compile());
                });

            var messages = new List<DbMessage>();
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user2,
                Sender = user1,
                Text = string.Concat(subjectText, "\n\r", messageText)
            });
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user1,
                Sender = user2,
                Text = string.Concat("RE: ", subjectText, "\n\r", messageText)
            });
            conversation.Messages = messages;
            conversationStore.Add(conversation);

            var service = InitService();

            //Act
            var conversations = service.GetConversationsByReceiver(Mapper.Map<User>(user2)).ToList();

            //Assert
            Assert.IsNotNull(conversations);
            Assert.IsTrue(conversations.Count > 0);
            Assert.IsTrue(conversations.Any(a => a.Id == 1));
            Assert.IsTrue(conversations.All(a => a.Request != null));
            Assert.IsTrue(conversations.All(a => a.Requester != null));
            Assert.IsTrue(conversations.All(a => a.Receiver != null));
            Assert.IsTrue(conversations.Any(a => a.Messages.Count() > 0));
        }

        [TestMethod()]
        public void GetConversationsWithUnreadMessagesTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };
            var user1 = new DbUser() { Id = Guid.NewGuid() };
            var user2 = new DbUser() { Id = Guid.NewGuid() };

            var subjectText = "Test request subject";
            var messageText = "Test request message";

            var conversationStore = new List<DbConversation>();
            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = user2,
                Requester = user1,
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };
            _conversationRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversationStore.Where(expr.Compile());
                });

            var messages = new List<DbMessage>();
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user2,
                Sender = user1,
                Text = string.Concat(subjectText, "\n\r", messageText)
            });
            messages.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user1,
                Sender = user2,
                Text = string.Concat("RE: ", subjectText, "\n\r", messageText)
            });
            conversation.Messages = messages;
            conversationStore.Add(conversation);

            var service = InitService();

            //Act
            var conversations = service.GetConversationsWithUnreadMessages(Mapper.Map<User>(user2)).ToList();

            //Assert
            Assert.IsNotNull(conversations);
            Assert.IsTrue(conversations.Count > 0);
            Assert.IsTrue(conversations.Any(a => a.HasRecieverUnreadMessages));
            Assert.IsTrue(conversations.Any(a => a.Id == 1));
            Assert.IsTrue(conversations.All(a => a.Request != null));
            Assert.IsTrue(conversations.All(a => a.Requester != null));
            Assert.IsTrue(conversations.All(a => a.Receiver != null));
            Assert.IsTrue(conversations.Any(a => a.Messages.Count() > 0));
        }

        [TestMethod()]
        public void GetMessagesTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };
            var user1 = new DbUser() { Id = Guid.NewGuid() };
            var user2 = new DbUser() { Id = Guid.NewGuid() };

            var subjectText = "Test request subject";
            var messageText = "Test request message";

            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = user2,
                Requester = user1,
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };
            _conversationRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversation;
                });


            var messagesStore = new List<DbMessage>();
            messagesStore.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user2,
                Sender = user1,
                Text = string.Concat(subjectText, "\n\r", messageText)
            });
            messagesStore.Add(new DbMessage()
            {
                Id = 1,
                Conversation = conversation,
                CreationDate = DateTime.Now,
                Receiver = user1,
                Sender = user2,
                Text = string.Concat("RE: ", subjectText, "\n\r", messageText)
            });

            _messageRepositoryMock.Setup(a => a.FindCollection(It.IsAny<Expression<Func<DbMessage, bool>>>()))
                .Returns<Expression<Func<DbMessage, bool>>>(expr =>
                {
                    return messagesStore.Where(expr.Compile());
                });

            var service = InitService();

            //Act
            var messages = service.GetMessages(conversation.Id, user2.Id).ToList();

            //Assert
            Assert.IsNotNull(messages);
            Assert.IsTrue(messages.Count > 0);
            Assert.IsTrue(messages.All(a => a.Sender != null));
            Assert.IsTrue(messages.All(a => a.Receiver != null));
            Assert.IsTrue(messages.Any(a => a.Sender.Id == user1.Id.ToString()));
            Assert.IsTrue(messages.Any(a => a.Receiver.Id == user1.Id.ToString()));
            Assert.IsTrue(messages.Any(a => a.Sender.Id == user2.Id.ToString()));
            Assert.IsTrue(messages.Any(a => a.Receiver.Id == user2.Id.ToString()));
        }

        [TestMethod()]
        public void AcceptConversationTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() {Id = 1};

            var userStore = new List<DbUser>();
            var user1Guid = Guid.NewGuid();
            var user2Guid = Guid.NewGuid();
            userStore.Add(new DbUser() { Id = user1Guid });
            userStore.Add(new DbUser() { Id = user2Guid });

            _usersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Returns<Expression<Func<DbUser, bool>>>(expr =>
                {
                    return userStore.FirstOrDefault(expr.Compile());
                });


            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = userStore[1],
                Requester = userStore[0],
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };

            _conversationRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversation;
                });
            _conversationRepositoryMock.Setup(s => s.Update(It.IsAny<DbConversation>())).Callback<DbConversation>(e =>
            {
                
            });

            var service = InitService();

            //Act
            service.AcceptConversation(conversation.Id);

            //Assert
            Assert.IsNotNull(conversation);
            Assert.AreEqual(conversation.Status, DbAccessRequestStatus.Accepted);
        }

        [TestMethod()]
        public void DeclineConversationTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };

            var userStore = new List<DbUser>();
            var user1Guid = Guid.NewGuid();
            var user2Guid = Guid.NewGuid();
            userStore.Add(new DbUser() { Id = user1Guid });
            userStore.Add(new DbUser() { Id = user2Guid });

            _usersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Returns<Expression<Func<DbUser, bool>>>(expr =>
                {
                    return userStore.FirstOrDefault(expr.Compile());
                });


            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = userStore[1],
                Requester = userStore[0],
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };

            _conversationRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversation;
                });
            _conversationRepositoryMock.Setup(s => s.Update(It.IsAny<DbConversation>())).Callback<DbConversation>(e =>
            {

            });

            var service = InitService();

            //Act
            service.DeclineConversation(conversation.Id, "some reason");

            //Assert
            Assert.IsNotNull(conversation);
            Assert.AreEqual(conversation.Status, DbAccessRequestStatus.Declined);
            Assert.AreEqual(conversation.StatusReason, "some reason");
        }

        [TestMethod()]
        public void AddNewMessageToConversationTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };

            var userStore = new List<DbUser>();
            var user1Guid = Guid.NewGuid();
            var user2Guid = Guid.NewGuid();
            userStore.Add(new DbUser() { Id = user1Guid });
            userStore.Add(new DbUser() { Id = user2Guid });

            _usersRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
                .Returns<Expression<Func<DbUser, bool>>>(expr =>
                {
                    return userStore.FirstOrDefault(expr.Compile());
                });


            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = userStore[1],
                Requester = userStore[0],
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };

            _conversationRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversation;
                });

            var messages = new List<DbMessage>();
            _messageRepositoryMock.Setup(s => s.Insert(It.IsAny<DbMessage>())).Callback<DbMessage>(e =>
            {
                e.Id = 1;
                messages.Add(e);
            });

            var service = InitService();

            //Act
            var message = service.AddNewMessageToConversation(conversation.Id, Mapper.Map<User>(userStore[0]),
                Mapper.Map<User>(userStore[1]), "some text", true);


            //Assert
            Assert.IsNotNull(message);
            Assert.IsTrue(messages.Count > 0);
            Assert.AreEqual(messages[0].Id, message.Id);
        }

        [TestMethod()]
        public void GetConversationByIdTest()
        {
            //Arrange
            var accessRequest = new DbAccessRequest() { Id = 1 };

            var userStore = new List<DbUser>();
            var user1Guid = Guid.NewGuid();
            var user2Guid = Guid.NewGuid();
            userStore.Add(new DbUser() { Id = user1Guid });
            userStore.Add(new DbUser() { Id = user2Guid });

            var conversationStore = new List<DbConversation>();
            var conversation = new DbConversation()
            {
                HasRecieverUnreadMessages = true,
                Id = 1,
                Receiver = userStore[1],
                Requester = userStore[0],
                Request = accessRequest,
                Status = DbAccessRequestStatus.Open
            };
            conversationStore.Add(conversation);
            _conversationRepositoryMock.Setup(s => s.Find(It.IsAny<Expression<Func<DbConversation, bool>>>()))
                .Returns<Expression<Func<DbConversation, bool>>>(expr =>
                {
                    return conversationStore.FirstOrDefault(expr.Compile());
                });

            var service = InitService();

            //Act
            var foundConversation = service.GetConversationById(1);


            //Assert
            Assert.IsNotNull(foundConversation);
            Assert.AreEqual(foundConversation.Id, 1);
        }

        private ConversationService InitService()
        {
            return new ConversationService(
                _accessRequestRepositoryMock.Object,
                _conversationRepositoryMock.Object,
                _usersRepositoryMock.Object,
                _messageRepositoryMock.Object,
                _filesRepositoryMock.Object,
                _annotationItemRepositoryMock.Object);
        }

        private ConversationService InitServiceWithouConstructorParams()
        {
            return new ConversationService(
                null,
                null,
                null,
                null,
                null,
                null);
        }
    }
}