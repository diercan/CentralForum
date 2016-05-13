using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string  TopicName { get; set; }
        public MessageType MessageType { get; set; } //Public,Private,How to
        public Guid PracticeGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string UserDisplayName { get; set; }
        public string PracticeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int UserRating { get; set; }
        public DateTime CreationDate {get; set;}
    }
}
