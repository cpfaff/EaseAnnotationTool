
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CAFE.Core.Security;

namespace CAFE.Core.Messaging
{
    /// <summary>
    /// Contract for conversations service that manage access request's conversation and messages
    /// </summary>
    [ContractClass(typeof(ConversationServiceContracts))]
    public interface IConversationService
    {
        /// <summary>
        /// Open new conversation for AccessRequest
        /// </summary>
        /// <param name="accessRequestId">Access request identifier</param>
        /// <param name="requesterId">Unique identifier of requester</param>
        /// <param name="receiverId">Unique identifier of requester conversation messages</param>
        /// <param name="subject">Subject of created conversation</param>
        /// <param name="message">Message of created conversation</param>
        /// <returns>New conversation</returns>
        Conversation OpenConversationFor(long accessRequestId, string requesterId, string receiverId, 
            string subject, string message);
        /// <summary>
        /// Returns collection of conversations for AccessRequest
        /// </summary>
        /// <param name="accessRequestId">Access request identifier</param>
        /// <returns>Collection of conversation</returns>
        IEnumerable<Conversation> GetConversationsFor(long accessRequestId);

        /// <summary>
        /// Returns conversations by sender (or initiator)
        /// </summary>
        /// <param name="sender">Sender (or initiator)</param>
        /// <returns>Found conversations</returns>
        IEnumerable<Conversation> GetConversationsBySender(User sender);

        /// <summary>
        /// Returns conversations by receiver
        /// </summary>
        /// <param name="receiver">Receiver</param>
        /// <returns>Found conversations</returns>
        IEnumerable<Conversation> GetConversationsByReceiver(User receiver);

        /// <summary>
        /// Returns conversations by receiver with unread messages
        /// </summary>
        /// <param name="receiver">Receiver</param>
        /// <returns>Found conversations</returns>
        IEnumerable<Conversation> GetConversationsWithUnreadMessages(User receiver);

        /// <summary>
        /// Acceptings caonversation (part of access request) and provide
        /// access for requested resource
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        KeyValuePair<Conversation, string> AcceptConversation(long conversationId);

        /// <summary>
        /// Declinings caonversation (part of access request) and prevent
        /// access for requested resource
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        /// <param name="reason">Text of reason of this decision</param>
        KeyValuePair<Conversation, string> DeclineConversation(long conversationId, string reason);

        /// <summary>
        /// Adds new message to conversation
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        /// <param name="sender">User that send message</param>
        /// <param name="receiver">User that receive message</param>
        /// <param name="text">Message text</param>
        /// <returns>New message</returns>
        Message AddNewMessageToConversation(long conversationId, User sender, User receiver, string text, bool hasReceiverUnreadMessages);

        Conversation GetConversationById(long conversationId);

        IEnumerable<Message> GetMessages(long conversationId, System.Guid userId);
    }

    /// <summary>
    /// Stub contracts implementation for IConversationService
    /// Need for validation purpose only
    /// </summary>
    [ContractClassFor(typeof(IConversationService))]
    public class ConversationServiceContracts : IConversationService
    {
        public Conversation OpenConversationFor(long accessRequestId, string requesterId, string receiverId, 
            string subject, string message)
        {
            Contract.Requires(accessRequestId > 0, "accessRequestId must be passed and bigger than 0");
            Contract.Requires(!string.IsNullOrEmpty(requesterId), "requesterId must be passed");
            Contract.Requires(!string.IsNullOrEmpty(receiverId), "receiverId must be passed");
            Contract.Requires(!string.IsNullOrEmpty(subject), "subject must be passed");

            throw new NotImplementedException();
        }

        public IEnumerable<Conversation> GetConversationsFor(long accessRequestId)
        {
            Contract.Requires(accessRequestId > 0, "accessRequestId must be passed and bigger than 0");

            throw new NotImplementedException();
        }

        public IEnumerable<Conversation> GetConversationsBySender(User sender)
        {
            Contract.Requires(sender != null, "sender must be passed");

            throw new NotImplementedException();
        }

        public IEnumerable<Conversation> GetConversationsByReceiver(User receiver)
        {
            Contract.Requires(receiver != null, "receiver must be passed");

            throw new NotImplementedException();
        }

        public IEnumerable<Conversation> GetConversationsWithUnreadMessages(User receiver)
        {
            Contract.Requires(receiver != null, "receiver must be passed");

            throw new NotImplementedException();
        }

        public KeyValuePair<Conversation, string> AcceptConversation(long conversationId)
        {
            Contract.Requires(conversationId > 0, "conversationId must be passed and bigger than 0");

            throw new NotImplementedException();
        }

        public KeyValuePair<Conversation, string> DeclineConversation(long conversationId, string reason)
        {
            Contract.Requires(conversationId > 0, "conversationId must be passed and bigger than 0");

            throw new NotImplementedException();
        }

        public Message AddNewMessageToConversation(long conversationId, User sender, User receiver, string text,
            bool hasReceiverUnreadMessages)
        {
            Contract.Requires(conversationId > 0, "conversationId must be passed and bigger than 0");
            Contract.Requires(sender != null, "sender must be passed");
            Contract.Requires(receiver != null, "receiver must be passed");
            Contract.Requires(!string.IsNullOrEmpty(text), "text must be passed");

            throw new NotImplementedException();
        }

        public Conversation GetConversationById(long conversationId)
        {
            Contract.Requires(conversationId > 0, "conversationId must be passed and bigger than 0");

            throw new NotImplementedException();
        }

        public IEnumerable<Message> GetMessages(long conversationId, Guid userId)
        {
            Contract.Requires(conversationId > 0, "conversationId must be passed and bigger than 0");
            Contract.Requires(userId != null, "userId must be passed");
            Contract.Requires(userId != Guid.Empty, "userId must be non 0000-0000-0000-0000");

            throw new NotImplementedException();
        }
    }
}
