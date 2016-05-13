using CentralForum.Client.Model;
using Models.Models;
using System;
using System.Collections.Generic;

namespace CentralForumApplication.Design
{
    public class DesignDataService : IDataService
    {

        public System.Collections.Generic.List<Message> GetPosts(string topicName, MessageType messageType, Guid practiceGuid)
        {
            var posts = new List<Message>();
            posts.AddRange(GenerateTestPost(1, 4));
            posts.AddRange(GenerateTestPost(2, 4));
            posts.AddRange(GenerateTestPost(3, 4));
            posts.AddRange(GenerateTestPost(4, 0));
            posts.AddRange(GenerateTestPost(5, 0));
            posts.AddRange(GenerateTestPost(6, 0));
            posts.AddRange(GenerateTestPost(6, 0));
            posts.AddRange(GenerateTestPost(7, 0));
            posts.AddRange(GenerateTestPost(9, 0));
            posts.AddRange(GenerateTestPost(10, 4));
            return posts;
        }

        private static List<Message> GenerateTestPost(int mainPostNumber, int repliesCount)
        {
            var postGroup = new List<Message>();
            var mainPost = new Message()
            {
                Title = "Post " + mainPostNumber,
                Description = "This is the text for post 1",
                Id = Guid.NewGuid()
            };
            postGroup.Add(mainPost);
            for(int i=0;i<repliesCount; i++)
            {
                postGroup.Add(new Message() {
                    Description = "Reply number " + (i + 1),
                    UserDisplayName = "iuliuz",
                    Title = "Replay" + (i + 1),
                    ParentId = mainPost.Id
                });
            }
            return postGroup;
        }

        public void PostMessage(Message post)
        {
        }

        public bool ThumbsUp(Guid ratedMessageId, Guid ratedUser, Guid ratingUser)
        {
            return true;
        }

        public bool ThumbsDown(Guid ratedMessageId, Guid ratedUser, Guid ratingUser)
        {
            return true;
        }
    }
}