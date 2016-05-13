using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralForum.Client.Messages
{
    public class RatingUpdatedMsg
    {
        public Guid MessageGuid { get; set; }
        public Guid UserGuid { get; set; }
        public RatingValues RatingValue { get; set; }
    }
}
