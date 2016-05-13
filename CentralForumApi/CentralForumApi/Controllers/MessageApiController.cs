using Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CentralForumApi.Controllers
{
    public class MessagesController : BaseApiController
    {
        [Route("test")]
        [HttpGet]
        public string Ping()
        {
            return "echo";
        }

        [Route("messages")]
        [HttpPost]
        public void PostMessage([FromBody]Message message)
        {
            dalUnitOfWork.MessageRepository.AddOrUpdate(message);
        }


        [Route("topics/{topicName}/{messageType=messageType}/{practiceGuid=practiceGuid}")]
        [HttpGet]
        public object GetMessages(string topicName, MessageType messageType, Guid practiceGuid)
        {
            var messages = dalUnitOfWork.MessageRepository.GetQ()
                .Where<Message>(m =>
                                    m.TopicName == topicName && (messageType & m.MessageType) == m.MessageType 
                                    && (m.MessageType != MessageType.Private || practiceGuid.Equals(m.PracticeGuid)))
                .Select(m => new
                {
                    Description = m.Description,
                    Id = m.Id,
                    MessageType = m.MessageType,
                    ParentId = m.ParentId,
                    PracticeGuid = m.PracticeGuid,
                    PracticeName = m.PracticeName,
                    Rating = dbContext.Rating
                                        .Where(r => r.RatedMessageGuid == m.Id)
                                        .Sum(r => (int?)r.Value) ?? 0,
                    Title = m.Title,
                    TopicName = m.TopicName,
                    UserDisplayName = m.UserDisplayName,
                    UserGuid = m.UserGuid,
                    UserRating = dbContext.Rating
                                        .Where(r => r.RatedUserGuid == m.UserGuid)
                                        .Sum(r => (int?)r.Value) ?? 0,
                    CreationDate = m.CreationDate
                });

            return messages.ToArray();
        }

        [Route("ratings")]
        [HttpPost]
        public bool RateMessage([FromBody]Rating rating)
        {
            if (!dalUnitOfWork.RatingRepository.GetQ()
                .Any(r => r.RatingUserGuid == rating.RatingUserGuid && r.RatedMessageGuid == rating.RatedMessageGuid))
            {
                dalUnitOfWork.RatingRepository.AddOrUpdate(rating);

                return true;
            }

            return false;
        }
    }
}