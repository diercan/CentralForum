using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid RatingUserGuid { get; set; }
        public Guid RatedMessageGuid { get; set; }
        public Guid RatedUserGuid { get; set; }
        public RatingValues Value { get; set; }
    }
}
