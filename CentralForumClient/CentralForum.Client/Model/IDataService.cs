using Models.Models;
using System;
using System.Collections.Generic;

namespace CentralForum.Client.Model
{
    public interface IDataService
    {
        List<Message> GetPosts(string topicName, MessageType messageType, Guid practiceGuid);
        void PostMessage(Message post);
        bool ThumbsUp(Guid ratedMessageId, Guid ratedUser, Guid ratingUser);
        bool ThumbsDown(Guid ratedMessageId, Guid ratedUser, Guid ratingUser);
    }
}
