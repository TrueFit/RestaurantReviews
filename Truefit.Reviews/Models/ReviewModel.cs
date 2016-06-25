using System;

namespace Truefit.Reviews.Models
{
    public class ReviewModel
    {
        public Guid Guid { get; set; }
        public Guid EntityGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
