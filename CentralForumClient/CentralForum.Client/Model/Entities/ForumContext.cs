using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralForum.Client.Model.Entities
{
    public class ForumContext
    {
        public Guid UserId { get; set; }
        public Guid PracticeId { get; set; }
        public string UserDisplayName { get; set; }
        public string PracticeDisplayName { get; set; }
        public string TopicName { get; set; }
        public string TopicDisplayName { get; set; }
    }
}
