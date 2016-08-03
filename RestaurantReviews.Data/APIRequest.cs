namespace RestaurantReviews.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APIRequest")]
    public partial class APIRequest
    {
        public int Id { get; set; }

        public DateTime AccessDateTime { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string Path { get; set; }

        [Required]
        [StringLength(1024)]
        public string PayloadDetails { get; set; }
    }
}
