using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Moq;
using CAFE.Core.Messaging;
using CAFE.Core.Resources;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;
using CAFE.Services.Security;

namespace CAFE.Tests.UnitTests.Security
{
	[TestClass()]
	public class AccessRequestServiceTests
	{
		private const string userOneId = "11111111-1111-1111-1111-111111111111";
		private const string userOneUsername = "User 1";
		private const string userOneEmail = "user1@domain.com";
		private const string userTwoId = "22222222-2222-2222-2222-222222222222";
		private const string userTwoUsername = "User 2";
		private const string userTwoEmail = "user2@domain.com";
		private const string userThreeId = "33333333-3333-3333-3333-333333333333";
		private const string userThreeUsername = "User 3";
		private const string userThreeEmail = "user3@domain.com";


		[TestInitialize]
		public void Setup()
		{
			Mapper.Initialize(Services.Mapping.MapperConfig.Init);
		}

		[TestMethod()]
		public void MakeAccessRequest_NoSubject_ArgumentException()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();
			var subject = string.Empty;
			var message = "Message";
			var requestorUser = getExistingUserData();

			var resources = new List<AccessibleResource> { };
			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 1,
				Kind = AccessibleResourceKind.File
			});

			var isArgumentExceptionThrown = false;

			try
			{
				accessRequestServiceObject.MakeRequest(subject, message, resources, requestorUser);
			}
			catch (ArgumentException exc)
			{
				isArgumentExceptionThrown = true;
			}
			finally
			{
				if (!isArgumentExceptionThrown)
				{
					Assert.Fail();
				}

				isArgumentExceptionThrown = false;
			}
		
		}

		[TestMethod()]
		public void MakeAccessRequest_NoResources_ArgumentException()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();
			var subject = "Subject";
			var message = "Message";
			var requestorUser = getExistingUserData();

			var isArgumentExceptionThrown = false;

			try
			{
				accessRequestServiceObject.MakeRequest(subject, message, null, requestorUser);
			}
			catch (ArgumentException exc)
			{
				isArgumentExceptionThrown = true;
			}
			finally
			{
				if (!isArgumentExceptionThrown)
				{
					Assert.Fail();
				}

				isArgumentExceptionThrown = false;
			}

			var resources = new List<AccessibleResource> { };

			try
			{
				accessRequestServiceObject.MakeRequest(subject, message, resources, requestorUser);
			}
			catch (ArgumentException exc)
			{
				isArgumentExceptionThrown = true;
			}
			finally
			{
				if (!isArgumentExceptionThrown)
				{
					Assert.Fail();
				}

				isArgumentExceptionThrown = false;
			}
		}

		[TestMethod()]
		public void MakeAccessRequest_NoUserOrNotFoundUser_Exception()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();
			var subject = "Subject";
			var message = "Message";
			var notExistingUser = getNotExistingUserData();
			var resources = getOneResource();
			

			var isArgumentExceptionThrown = false;

			try
			{
				accessRequestServiceObject.MakeRequest(subject, message, resources, notExistingUser);
			}
			catch (Exception exc)
			{
				isArgumentExceptionThrown = true;
			}
			finally
			{
				if (!isArgumentExceptionThrown)
				{
					Assert.Fail();
				}

				isArgumentExceptionThrown = false;
			}
		}

		[TestMethod()]
		public void MakeAccessRequest_OneResource_OneResourceOneConversation()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();
			var subject = "Subject";
			var message = "Message";
			var existingUser = getExistingUserData();
			var resources = getOneResource();
			var numberOfResourceOwners = 1;
			var accessRequest = accessRequestServiceObject.MakeRequest(subject, message, resources, existingUser);

			Assert.IsTrue(
				accessRequest.RequestedResources.ToList().Count == resources.Count &&
				accessRequest.Conversations.ToList().Count == numberOfResourceOwners
				);
		}

		[TestMethod()]
		public void MakeAccessRequest_MultipleResourceSameOwner_MultipleResourcesOneConversation()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();
			var subject = "Subject";
			var message = "Message";
			var existingUser = getExistingUserData();
			var resources = getMultipleResourcesSingleOwner();
			var numberOfResourceOwners = 1;
			var accessRequest = accessRequestServiceObject.MakeRequest(subject, message, resources, existingUser);

			Assert.IsTrue(
				accessRequest.RequestedResources.ToList().Count == resources.Count &&
				accessRequest.Conversations.ToList().Count == numberOfResourceOwners
				);
		}

		[TestMethod()]
		public void MakeAccessRequest_MultipleResourceDifferentOwners_MultipleResourcesMultipleConversations()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();
			var subject = "Subject";
			var message = "Message";
			var existingUser = getExistingUserData();
			var resources = getMultipleResourcesDifferentOwners();
			var numberOfResourceOwners = 2;
			var accessRequest = accessRequestServiceObject.MakeRequest(subject, message, resources, existingUser);

			Assert.IsTrue(
				accessRequest.RequestedResources.ToList().Count == resources.Count &&
				accessRequest.Conversations.ToList().Count == numberOfResourceOwners
				);
		}


		[TestMethod()]
		public void GetAccessRequest_ExistingRequest_ReturnRequest()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();

			var result = accessRequestServiceObject.GetAccessRequest(1);

			Assert.IsTrue(result != null);
		}

		[TestMethod()]
		public void GetAccessRequest_NotExistingRequest_Exception()
		{
			var accessRequestServiceObject = getMockedAccessRequestService();

			var isArgumentExceptionThrown = false;

			try
			{
				var result = accessRequestServiceObject.GetAccessRequest(999);
			}
			catch (Exception exc)
			{
				isArgumentExceptionThrown = true;
			}
			finally
			{
				if (!isArgumentExceptionThrown)
				{
					Assert.Fail();
				}

				isArgumentExceptionThrown = false;
			}
		}

		#region internal implementation

		private IRepository<DbUser> _userRepository;
		protected IRepository<DbUser> UserRepository
		{
			get
			{
				if (_userRepository == null)
				{
					_userRepository = getUserRepository();
				}

				return _userRepository;
			}
		}

		private IRepository<DbUser> getUserRepository()
		{
			var userStore = new List<DbUser>()
			{
				new DbUser
				{
					UserName = userOneUsername,
					Email = userOneEmail,
					Id = new Guid(userOneId)
				},
				new DbUser
				{
					UserName = userTwoUsername,
					Email = userTwoEmail,
					Id = new Guid(userTwoId)
				},
				new DbUser
				{
					UserName = userThreeUsername,
					Email = userThreeEmail,
					Id = new Guid(userThreeId)
				}
			};

			var userRepositoryMock = new Mock<IRepository<DbUser>>();
			userRepositoryMock.Setup(a => a.Select()).Returns(userStore.AsQueryable());
			userRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbUser, bool>>>()))
				.Returns<Expression<Func<DbUser, bool>>>(expr =>
				{
					return userStore.FirstOrDefault(expr.Compile());
				});
			return userRepositoryMock.Object;
		}

		private IRepository<DbAccessRequest> getAccessRequestRepository()
		{

			var conversations = new List<DbConversation>
			{
				new DbConversation
				{
					Id = 1,
					Messages = new List<DbMessage> { },
					Receiver = UserRepository.Find(x => x.Id.ToString() == userTwoId),
					Request = new DbAccessRequest { Id = 12},
					Requester = UserRepository.Find(x => x.Id.ToString() == userOneId),
					Status = DbAccessRequestStatus.Open,
					StatusReason = ""
				}

			};

			var accessStore = new List<DbAccessRequest>()
			{
				new DbAccessRequest
				{
					Conversations = conversations,
					CreationDate = new System.DateTime(),
					Id = 1,
					RequestedResources = new List<DbAccessibleResource>(),
					RequestMessage = "Message",
					RequestSubject = "Subject"					
				}
			};

			var accessRequestRepositoryMock = new Mock<IRepository<DbAccessRequest>>();
			accessRequestRepositoryMock.Setup(a => a.Select()).Returns(accessStore.AsQueryable());
			accessRequestRepositoryMock.Setup(a => a.Find(It.IsAny<Expression<Func<DbAccessRequest, bool>>>()))
				.Returns<Expression<Func<DbAccessRequest, bool>>>(expr =>
				{
					return accessStore.FirstOrDefault(expr.Compile());
				});
			return accessRequestRepositoryMock.Object;
		}

		private IAccessibleResourceDescriptor getAccessibleresourceDescriptorOwnedByUserTwoMock()
		{
			var accessibleresourceDescriptor = new Mock<IAccessibleResourceDescriptor>();
			accessibleresourceDescriptor.Object.Description = "Description";
			accessibleresourceDescriptor.Object.Id = new Guid();
			accessibleresourceDescriptor.Object.Name = "Name";

			var ownerUser = getExistingUserDataSecondUser();

			accessibleresourceDescriptor.Setup(x => x.OwnerId).Returns(ownerUser.Id);
			accessibleresourceDescriptor.Setup(x => x.OwnerName).Returns(ownerUser.Name);
			accessibleresourceDescriptor.Setup(x => x.Description).Returns("resource description");
			accessibleresourceDescriptor.Setup(x => x.Name).Returns("resource name");
			var resourceGuid = new Guid();
			accessibleresourceDescriptor.Setup(x => x.Id).Returns(resourceGuid);

			return accessibleresourceDescriptor.Object;

		}

		private IAccessibleResourceDescriptor getAccessibleResourceDescriptorOwnedByUserThreeMock()
		{
			var accessibleresourceDescriptor = new Mock<IAccessibleResourceDescriptor>();
			accessibleresourceDescriptor.Object.Description = "Description";
			accessibleresourceDescriptor.Object.Id = new Guid();
			accessibleresourceDescriptor.Object.Name = "Name";

			var ownerUser = getExistingUserDataThirdUser();

            accessibleresourceDescriptor.Setup(x => x.OwnerId).Returns(ownerUser.Id);
            accessibleresourceDescriptor.Setup(x => x.OwnerName).Returns(ownerUser.Name);
            accessibleresourceDescriptor.Setup(x => x.Description).Returns("resource description");
			accessibleresourceDescriptor.Setup(x => x.Name).Returns("resource name");
			var resourceGuid = new Guid();
			accessibleresourceDescriptor.Setup(x => x.Id).Returns(resourceGuid);

			return accessibleresourceDescriptor.Object;
		}

		private AccessRequestService getMockedAccessRequestService()
		{
			var accessRequestRepository = getAccessRequestRepository();
			var resourcesProviderMock = new Mock<IAccessibleResourcesProvider>();
			var conversationRepositoryMock = new Mock<IRepository<DbConversation>>();



			var userFilesRepositoryMock = new Mock<IRepository<DbUserFile>>();
			var annotationItemsRepositoryMock = new Mock<IRepository<DbAnnotationItem>>();
			var conversationServiceMock = new Mock<IConversationService>();

			var mockedAccessRequestService = new AccessRequestService(
				accessRequestRepository,
				resourcesProviderMock.Object,
				conversationRepositoryMock.Object,
				UserRepository,
				userFilesRepositoryMock.Object,
				annotationItemsRepositoryMock.Object,
				conversationServiceMock.Object
				);

			return mockedAccessRequestService;
		}

		private User getExistingUserData()
		{
			var existingUser = new User
			{
				UserName = userOneUsername,
				Email = userOneEmail,
				Id = userOneId
			};

			return existingUser;
		}

		private User getExistingUserDataSecondUser()
		{
			var existingUser = new User
			{
				UserName = userTwoUsername,
				Email = userTwoEmail,
				Id = userTwoId
			};

			return existingUser;
		}

		private User getExistingUserDataThirdUser()
		{
			var existingUser = new User
			{
				UserName = userThreeUsername,
				Email = userThreeEmail,
				Id = userThreeId
			};

			return existingUser;
		}

		private User getNotExistingUserData()
		{
			var notExistingUser = new User
			{
				UserName = "User 99",
				Email = "user99@domain.com",
				Id = "99999999-9999-9999-9999-999999999999"
			};

			return notExistingUser;
		}

		private List<AccessibleResource> getOneResource()
		{
			var resources = new List<AccessibleResource> { };
			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 1,
				Kind = AccessibleResourceKind.File				
			});

			return resources;
		}

		private List<AccessibleResource> getMultipleResourcesSingleOwner()
		{
			var resources = new List<AccessibleResource> { };

			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 1,
				Kind = AccessibleResourceKind.File
			});

			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 2,
				Kind = AccessibleResourceKind.File
			});

			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 3,
				Kind = AccessibleResourceKind.AnnotationItem
			});

			return resources;
		}

		private List<AccessibleResource> getMultipleResourcesDifferentOwners()
		{
			var resources = new List<AccessibleResource> { };

			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 1,
				Kind = AccessibleResourceKind.File
			});

			resources.Add(new AccessibleResource
			{
				Content = getAccessibleResourceDescriptorOwnedByUserThreeMock(),
				Id = 2,
				Kind = AccessibleResourceKind.File
			});

			resources.Add(new AccessibleResource
			{
				Content = getAccessibleresourceDescriptorOwnedByUserTwoMock(),
				Id = 3,
				Kind = AccessibleResourceKind.AnnotationItem
			});

			return resources;
		}

		#endregion
	}
}
