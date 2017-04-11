
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CAFE.Core.Messaging;
using CAFE.Core.Resources;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;

namespace CAFE.Services.Security
{
	/// <summary>
	/// Access requests service that manage of access for resources
	/// </summary>
	public class AccessRequestService : IAccessRequestService
	{
		private readonly IRepository<DbAccessRequest> _accessRequestRepository;
		private readonly IAccessibleResourcesProvider _accessibleResourcesProvider;
		private readonly IRepository<DbConversation> _conversationRepository;
		private readonly IRepository<DbUser> _usersRepository;
		private readonly IRepository<DbUserFile> _filesRepository;
		private readonly IRepository<DbAnnotationItem> _annotationItemsRepository;
		private readonly IConversationService _conversationService;

		/// <summary>
		/// Constructor with dependencies
		/// </summary>
		/// <param name="accessRequestRepository">Repository of Access requests</param>
		/// <param name="accessibleResourcesProvider">Provider for resolve kind of resources</param>
		/// <param name="conversationRepository">Conversations repository</param>
		/// <param name="usersRepository">Users repository</param>
		/// <param name="annotationItemsRepository">Annotation items repository</param>
		/// <param name="conversationService">Conversations service</param>
		/// <param name="filesRepository">User's files repository</param>
		public AccessRequestService(IRepository<DbAccessRequest> accessRequestRepository, 
			IAccessibleResourcesProvider accessibleResourcesProvider,
			IRepository<DbConversation> conversationRepository, IRepository<DbUser> usersRepository,
			IRepository<DbUserFile> filesRepository, IRepository<DbAnnotationItem> annotationItemsRepository,
			IConversationService conversationService)
		{
			_accessRequestRepository = accessRequestRepository;
			_accessibleResourcesProvider = accessibleResourcesProvider;
			_conversationRepository = conversationRepository;
			_usersRepository = usersRepository;
			_filesRepository = filesRepository;
			_annotationItemsRepository = annotationItemsRepository;
			_conversationService = conversationService;
		}

		/// <summary>
		/// Make new access request for collection of resources
		/// </summary>
		/// <param name="subject">Text subject for receiver</param>
		/// <param name="message">Text message for receiver</param>
		/// <param name="resources">List of asking resources</param>
		/// <param name="requester">User that sending request</param>
		/// <returns></returns>
		public AccessRequest MakeRequest(string subject, string message, 
			IEnumerable<AccessibleResource> resources, User requester)
		{
			if (string.IsNullOrWhiteSpace(subject))
			{
				throw new ArgumentException(
					"Argument must contain some non-empty value", $"{nameof(subject)}");
			}

			if (resources == null || resources.ToList().Count < 1)
			{
				throw new ArgumentException(
					"Argument does not contain any resources", $"{nameof(resources)}");
			}

			if (requester == null)
			{
				throw new ArgumentException(
					"Argument cannot be null", $"{nameof(requester)}");
			}

			Guid requesterGuid;
			if (!Guid.TryParse(requester.Id, out requesterGuid))
			{
				throw new ArgumentException(
					"The User's ID was not recognized as GUID", $"{nameof(requester)}");
			}

			var isRequestorExists = _usersRepository.Find(x => x.Id == requesterGuid) != null;
			if (!isRequestorExists)
			{
				throw new Exception(
					"Requester-User was not found");
			}

			var newAccessRequest = new DbAccessRequest();
			newAccessRequest.CreationDate = System.DateTime.Now;
			newAccessRequest.RequestMessage = message;
			newAccessRequest.RequestSubject = subject;

			_accessRequestRepository.Insert(newAccessRequest);

			var resourceList = resources.ToList();
			var groupedResourcesByOwner = resourceList.GroupBy(g => g.Content.OwnerId);
			foreach (var group in groupedResourcesByOwner)
			{

				//Open conversation after request created
				var conversation = _conversationService.OpenConversationFor(newAccessRequest.Id, requester.Id, group.Key,
					subject, message);

                ((IList<DbConversation>)newAccessRequest.Conversations).Add(
                    _conversationRepository.Find(f => f.Id == conversation.Id));

            }
			foreach (var accessibleResource in resourceList)
			{
				var newAccessibleResource = new DbAccessibleResource();
				newAccessibleResource.Kind =
					Mapper.Map<AccessibleResourceKind, DbAccessibleResourceKind>(accessibleResource.Kind);
				newAccessibleResource.ResourceId = accessibleResource.Content.Id;
				newAccessibleResource.Owner =
					_usersRepository.Find(f => f.Id.ToString() == accessibleResource.Content.OwnerId);
                ((IList<DbAccessibleResource>)newAccessRequest.RequestedResources).Add(newAccessibleResource);
            }

			_accessRequestRepository.Update(newAccessRequest);

			var mappedRequest = Mapper.Map(newAccessRequest, new AccessRequest());
            ((IList<AccessibleResource>)mappedRequest.RequestedResources).Clear();
            foreach (var dbAccessibleResource in newAccessRequest.RequestedResources)
            {
                ((IList<AccessibleResource>)mappedRequest.RequestedResources).Add(
                    _accessibleResourcesProvider.GetResource(
                        Mapper.Map<DbAccessibleResourceKind, AccessibleResourceKind>(dbAccessibleResource.Kind),
                        dbAccessibleResource.ResourceId));
            }


            return mappedRequest;
		}

		/// <summary>
		/// Returns access request by identifier
		/// </summary>
		/// <param name="accessRequestId">Access request identifier</param>
		/// <returns>Found access request</returns>
		public AccessRequest GetAccessRequest(long accessRequestId)
		{
			var dbAccessRequest = _accessRequestRepository.Find(f => f.Id == accessRequestId);

			if (dbAccessRequest == null)
			{
				throw new Exception("DbAccessRequest not found");
			}

			var mappedRequest = Mapper.Map(dbAccessRequest, new AccessRequest());

            //fill resources
            ((List<AccessibleResource>)mappedRequest.RequestedResources).Clear();
            ((List<AccessibleResource>)mappedRequest.RequestedResources).AddRange(
                ConvertFromDbToDomainAccessibleResources(dbAccessRequest.RequestedResources));

            return mappedRequest;
		}

		private IEnumerable<AccessibleResource> ConvertFromDbToDomainAccessibleResources(
			IEnumerable<DbAccessibleResource> resources)
		{
			var resourcesList = new List<AccessibleResource>();
			var dbResourcesList = resources.ToList();
			foreach (var dbAccessibleResource in dbResourcesList)
			{
				var newAccessibleResource = new AccessibleResource();
				newAccessibleResource.Kind =
					Mapper.Map<DbAccessibleResourceKind, AccessibleResourceKind>(dbAccessibleResource.Kind);
				newAccessibleResource.Id = dbAccessibleResource.Id;
			    newAccessibleResource.ResourceId = dbAccessibleResource.ResourceId;

                if (dbAccessibleResource.Kind == DbAccessibleResourceKind.File)
                {
                    var dbFile = _filesRepository.Find(f => f.Id == dbAccessibleResource.ResourceId);
                    if (dbFile != null)
                    {
                        var mappedFile = Mapper.Map(dbFile, new UserFile());

                        newAccessibleResource.Content = mappedFile;
                    }
                    else
                    {
                        newAccessibleResource.Content = new UserFile() {OwnerId = Guid.Empty.ToString(), OwnerName = "UNKNOWN", Name = "REMOVED", Description = "[REMOVED RESOURCE]"};
                    }
                }
				else
				{
					//var mappedAnnotationItem =
					//	Mapper.Map(_annotationItemsRepository.Find(f => f.Id == dbAccessibleResource.ResourceId),
					//		new Core.Integration.AnnotationItem());
                    var dbAnnotation = _annotationItemsRepository.Find(f => f.Id == dbAccessibleResource.ResourceId);
				    if(dbAnnotation != null)
				        newAccessibleResource.Content = dbAnnotation;
                    else
                        newAccessibleResource.Content = new DbAnnotationItem() { OwnerId = Guid.Empty.ToString(), OwnerName = "UNKNOWN" };

                }
				resourcesList.Add(newAccessibleResource);
			}
			return resourcesList;
		} 
	}
}
