namespace RestaurantReviews.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        public int RestaurantID { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool IsPositive { get; set; }

        [Required]
        public string Comments { get; set; }
    }
}
