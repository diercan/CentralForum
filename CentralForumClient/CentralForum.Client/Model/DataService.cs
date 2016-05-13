using Models.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CentralForum.Client.Model
{
    public class DataService : IDataService
    {
        private HttpClient client = new HttpClient();

        public DataService()
        {
#if DEBUG
            client.BaseAddress = new Uri("http://localhost:5144");
#else
            client.BaseAddress = new Uri("http://centralforum.azurewebsites.net");
#endif
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get methods
        public List<Message> GetPosts(string topicName, MessageType messageType, Guid practiceGuid)
        {
            HttpResponseMessage response = client.GetAsync(string.Format("/topics/{0}?messageType={1}&practiceGuid={2}", topicName, (int)messageType, practiceGuid)).Result;

            var input = response.Content.ReadAsStringAsync().Result;

            var messages = JsonConvert.DeserializeObject<Message[]>(input);
            return messages.ToList();
        }

        // Create methods
        public void PostMessage(Message post)
        {
            Post("/messages", post);
        }

        public bool ThumbsUp(Guid ratedMessageId, Guid ratedUser, Guid ratingUser)
        {
            var rating = new Rating()
            {
                Id = Guid.NewGuid(),
                RatedMessageGuid = ratedMessageId,
                RatedUserGuid = ratedUser,
                RatingUserGuid = ratingUser,
                Value = RatingValues.Up
            };
            return (bool)Post("/ratings", rating);
        }

        public bool ThumbsDown(Guid ratedMessageId, Guid ratedUser, Guid ratingUser)
        {
            var rating = new Rating()
            {
                Id = Guid.NewGuid(),
                RatedMessageGuid = ratedMessageId,
                RatedUserGuid = ratedUser,
                RatingUserGuid = ratingUser,
                Value = RatingValues.Down
            };
            return (bool)Post("/ratings", rating);
        }

        private object Post(string relativeUri, object payload)
        {
            var output = JsonConvert.SerializeObject(payload);

            HttpResponseMessage response = client.PostAsync(
                        relativeUri
                        , new StringContent(output, Encoding.Default
                        , "application/json")
            ).Result;
            return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
        }
    }
}