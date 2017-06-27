
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AutoMapper;
using CAFE.Core.Messaging;
using CAFE.Core.Security;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models;

namespace CAFE.Services.Messaging
{
    /// <summary>
    /// Conversations service that manage access request's conversation and messages
    /// </summary>
    public class ConversationService : IConversationService
    {
        private readonly IRepository<DbAccessRequest> _accessRequestRepository;
        private readonly IRepository<DbConversation> _conversationRepository;
        private readonly IRepository<DbUser> _userRepository;
        private readonly IRepository<DbMessage> _messagesRepository;
        private readonly IRepository<DbUserFile> _filesRepository;
        private readonly IRepository<DbAnnotationItem> _annotationItemsRepository;
        private readonly IRepository<DbAnnotationItemAccessibleUsers> _accessibleAnnotationUsers;


        /// <summary>
        /// Constructor with dependencies
        /// </summary>
        /// <param name="accessRequestRepository">Access requests repository</param>
        /// <param name="conversationRepository">Conversations repository</param>
        /// <param name="userRepository">Users repository</param>
        /// <param name="messagesRepository">User's messages repository</param>
        /// <param name="filesRepository">User files repository</param>
        /// <param name="annotationItemsRepository">Annotation items repository</param>
        public ConversationService(IRepository<DbAccessRequest> accessRequestRepository,
            IRepository<DbConversation> conversationRepository, IRepository<DbUser> userRepository,
            IRepository<DbMessage> messagesRepository, IRepository<DbUserFile> filesRepository,
            IRepository<DbAnnotationItem> annotationItemsRepository,
            IRepository<DbAnnotationItemAccessibleUsers> accessibleAnnotationUsers)

        {
            Contract.Requires(accessRequestRepository != null);
            Contract.Requires(conversationRepository != null);
            Contract.Requires(userRepository != null);
            Contract.Requires(messagesRepository != null);
            Contract.Requires(filesRepository != null);
            Contract.Requires(annotationItemsRepository != null);

            _accessRequestRepository = accessRequestRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
            _filesRepository = filesRepository;
            _annotationItemsRepository = annotationItemsRepository;
            _accessibleAnnotationUsers = accessibleAnnotationUsers;
        }

        /// <summary>
        /// Open new conversation for AccessRequest
        /// </summary>
        /// <param name="accessRequestId">Access request identifier</param>
        /// <param name="requesterId">Unique identifier of requester</param>
        /// <param name="receiverId">Unique identifier of requester conversation messages</param>
        /// <param name="subject">Subject of created conversation</param>
        /// <param name="message">Message of created conversation</param> 
        /// <returns>New conversation</returns>
        public Conversation OpenConversationFor(long accessRequestId, string requesterId, string receiverId,
            string subject, string message)
        {
            var dbAccessRequest = _accessRequestRepository.Find(f => f.Id == accessRequestId);

            var dbRequester = _userRepository.Find(f => f.Id.ToString() == requesterId);
            var dbReceiver = _userRepository.Find(f => f.Id.ToString() == receiverId);

            var newConversation = new DbConversation();
            newConversation.Request = dbAccessRequest;
            newConversation.Requester = dbRequester;
            newConversation.Receiver = dbReceiver;
            newConversation.Status = DbAccessRequestStatus.Open;
            newConversation.HasRecieverUnreadMessages = true;
            _conversationRepository.Insert(newConversation);

            //create first initial message
            var newMessage = new DbMessage();
            newMessage.CreationDate = System.DateTime.Now;
            newMessage.Sender = dbRequester;
            newMessage.Receiver = dbReceiver;
            newMessage.Text = message;
            newMessage.Conversation = newConversation;
            _messagesRepository.Insert(newMessage);

            var mappedConversation = Mapper.Map(newConversation, new Conversation());

            return mappedConversation;
        }

        /// <summary>
        /// Returns collection of conversations for AccessRequest
        /// </summary>
        /// <param name="accessRequestId">Access request identifier</param>
        /// <returns>Collection of conversation</returns>
        public IEnumerable<Conversation> GetConversationsFor(long accessRequestId)
        {
            var dbConversations = _conversationRepository.FindCollection(f => f.Request.Id == accessRequestId);
            var mappedConversations = Mapper.Map<IEnumerable<DbConversation>, IEnumerable<Conversation>>(dbConversations);

            return mappedConversations;
        }
        /// <summary>
        /// Returns conversations by sender (or initiator)
        /// </summary>
        /// <param name="sender">Sender (or initiator)</param>
        /// <returns>Found conversations</returns>
        public IEnumerable<Conversation> GetConversationsBySender(User sender)
        {
            var dbRequests = _conversationRepository.FindCollection(s => s.Requester.Id.ToString() == sender.Id).ToList();
            var mappedRequests = Mapper.Map<IEnumerable<DbConversation>, IEnumerable<Conversation>>(dbRequests);

            return mappedRequests;
        }
        /// <summary>
        /// Returns conversations by receiver
        /// </summary>
        /// <param name="receiver">Receiver</param>
        /// <returns>Found conversations</returns>
        public IEnumerable<Conversation> GetConversationsByReceiver(User receiver)
        {
            var dbRequests = _conversationRepository.FindCollection(s => s.Receiver.Id.ToString() == receiver.Id).ToList();
            var mappedRequests = Mapper.Map<IEnumerable<DbConversation>, IEnumerable<Conversation>>(dbRequests);

            return mappedRequests;
        }

        /// <summary>
        /// Returns conversations by receiver with unread messages
        /// </summary>
        /// <param name="receiver">Receiver</param>
        /// <returns>Found conversations</returns>
        public IEnumerable<Conversation> GetConversationsWithUnreadMessages(User receiver)
        {
            var dbConversations = _conversationRepository.
                FindCollection(
                s => s.Receiver.Id.ToString() == receiver.Id &&
                s.HasRecieverUnreadMessages == true &&
                s.Status == DbAccessRequestStatus.Open).
                Take(10).
                ToList();

            foreach (var dbConversation in dbConversations)
            {
                var dbMessage = _messagesRepository.FindCollection(m => m.Conversation.Id == dbConversation.Id).ToList().LastOrDefault();
                //var dbMessage = _messagesRepository.FindLast(m => m.Conversation.Id == dbConversation.Id);
                dbConversation.Messages = new List<DbMessage>() { dbMessage };
            }

            var mappedRequests = Mapper.Map<IEnumerable<DbConversation>, IEnumerable<Conversation>>(dbConversations);

            return mappedRequests;
        }

        /// <summary>
        /// Returns conversations by receiver
        /// </summary>
        /// <param name="conversationId">Conversation</param>
        /// <returns>Found conversations</returns>
        public IEnumerable<Message> GetMessages(long conversationId, System.Guid userId)
        {
            var dbConversation = _conversationRepository.Find(c => c.Id == conversationId);
            var dbConversationNormalized = Mapper.Map<DbConversation, DbConversation>(dbConversation);
            if (dbConversationNormalized.Receiver.Id == userId)
                dbConversationNormalized.HasRecieverUnreadMessages = false;

            //System.Diagnostics.Debug.WriteLine("DEbug Info = " + (dbConversation.Request == null).ToString());

            _conversationRepository.Update(dbConversationNormalized);

            var dbMessages = _messagesRepository.FindCollection(m => m.Conversation.Id == conversationId);
            var mappedMessages = Mapper.Map<IEnumerable<DbMessage>, IEnumerable<Message>>(dbMessages);

            return mappedMessages;
        }
        /// <summary>
        /// Acceptings caonversation (part of access request) and provide
        /// access for requested resource
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        public KeyValuePair<Conversation, string> AcceptConversation(long conversationId)
        {
            var request = _conversationRepository.Find(f => f.Id == conversationId);
            request.Requester = _userRepository.Find(u => u.Id == request.Requester.Id);
            request.Receiver = _userRepository.Find(u => u.Id == request.Receiver.Id);
            var acceptRequest = request.Request;
            var resourcesNames = new List<string>();

            foreach (var resource in acceptRequest.RequestedResources.Where(w => w.Owner.Id == request.Receiver.Id))
            {
                if (resource.Kind == DbAccessibleResourceKind.File)
                {
                    var file = _filesRepository.Find(f => f.Id == resource.ResourceId);
                    if (!request.Requester.AccessibleFiles.Any(f => f.Id == file.Id))
                        request.Requester.AccessibleFiles.Add(file);

                    request.Requester.AccessibleFiles.Add(file);
                    _userRepository.Update(request.Requester);
                    resourcesNames.Add(file.Name);
                }
                else
                {
                    var annotaionItem = _annotationItemsRepository.Find(f => f.Id == resource.ResourceId);
                    _accessibleAnnotationUsers.Insert(new DbAnnotationItemAccessibleUsers()
                    {
                        AnnotationItem = annotaionItem,
                        User = request.Requester,
                        Id = Guid.NewGuid()
                    });
                    resourcesNames.Add(annotaionItem.Object.References.Descriptions?[0]?.Title);
                    var resources = annotaionItem.Object.Resources?[0]?.OfflineResources.ToList();
                    resources.ForEach(r =>
                    {
                        var fileId = r.FilePath.Split('/').Last();
                        var file = _filesRepository.Find(f => f.Id.ToString() == fileId);
                        if (!request.Requester.AccessibleFiles.Any(f => f.Id.ToString() == fileId))
                        {
                            file.AccessMode = DbUserFile.DbFileAccessMode.Explicit;
                            _filesRepository.Update(file);

                            request.Requester.AccessibleFiles.Add(file);
                            _userRepository.Update(request.Requester);
                        }
                    });
                }
            }

            request.Status = DbAccessRequestStatus.Accepted;
            _conversationRepository.Update(request);

            var result = new KeyValuePair<Conversation, string>(Mapper.Map<Conversation>(request), String.Join(",", resourcesNames));
            return result;
        }
        /// <summary>
        /// Declinings caonversation (part of access request) and prevent
        /// access for requested resource
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        /// <param name="reason">Text of reason of this decision</param>
        public KeyValuePair<Conversation, string> DeclineConversation(long conversationId, string reason)
        {
            var resourcesNames = new List<string>();
            var request = _conversationRepository.Find(f => f.Id == conversationId);
            request.Requester = _userRepository.Find(u => u.Id == request.Requester.Id);
            request.Receiver = _userRepository.Find(u => u.Id == request.Receiver.Id);

            request.StatusReason = reason;
            request.Status = DbAccessRequestStatus.Declined;
            _conversationRepository.Update(request);

            foreach (var resource in request.Request.RequestedResources.Where(w => w.Owner.Id == request.Receiver.Id))
            if (resource.Kind == DbAccessibleResourceKind.File)
            {
                var file = _filesRepository.Find(f => f.Id == resource.ResourceId);
                resourcesNames.Add(file.Name);
            }
            else
            {
                var annotaionItem = _annotationItemsRepository.Find(f => f.Id == resource.ResourceId);
                resourcesNames.Add(annotaionItem.Object.References.Descriptions?[0]?.Title);
            }
            var result = new KeyValuePair<Conversation, string>(Mapper.Map<Conversation>(request), String.Join(",", resourcesNames));
            return result;
        }

        /// <summary>
        /// Adds new message to conversation
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        /// <param name="sender">User that send message</param>
        /// <param name="receiver">User that receive message</param>
        /// <param name="text">Messagetext</param>
        /// <returns>New message</returns>
        public Message AddNewMessageToConversation(long conversationId, User sender, User receiver, string text, bool hasRequestRecieverUnreadMessages)
        {
            var dbConversation = _conversationRepository.Find(f => f.Id == conversationId);
            if (hasRequestRecieverUnreadMessages)
                dbConversation.HasRecieverUnreadMessages = true;

            var dbSender = _userRepository.Find(f => f.Id.ToString() == sender.Id);
            var dbReceiver = _userRepository.Find(f => f.Id.ToString() == receiver.Id);

            var newMessage = new DbMessage();
            newMessage.CreationDate = System.DateTime.Now;
            newMessage.Sender = dbSender;
            newMessage.Receiver = dbReceiver;
            newMessage.Text = text;
            newMessage.Conversation = dbConversation;

            _messagesRepository.Insert(newMessage);

            var mappedMessage = Mapper.Map(newMessage, new Message());

            return mappedMessage;
        }

        public Conversation GetConversationById(long conversationId)
        {
            var dbConversation = _conversationRepository.Find(f => f.Id == conversationId);

            dbConversation.Requester = _userRepository.Find(u => u.Id == dbConversation.Requester.Id);
            dbConversation.Receiver = _userRepository.Find(u => u.Id == dbConversation.Receiver.Id);

            return Mapper.Map(dbConversation, new Conversation());
        }
    }
}
