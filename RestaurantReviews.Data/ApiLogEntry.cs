namespace RestaurantReviews.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApiLogEntry")]
    public partial class ApiLogEntry
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Application { get; set; }

        [StringLength(50)]
        public string User { get; set; }

        [StringLength(50)]
        public string Machine { get; set; }

        [StringLength(20)]
        public string RequestIPAddress { get; set; }

        [StringLength(200)]
        public string RequestContentType { get; set; }

        public string RequestContentBody { get; set; }

        [StringLength(255)]
        public string RequestUri { get; set; }

        [StringLength(10)]
        public string RequestMethod { get; set; }

        [StringLength(500)]
        public string RequestRouteTemplate { get; set; }

        [StringLength(500)]
        public string RequestRouteData { get; set; }

        [StringLength(500)]
        public string RequestHeaders { get; set; }

        public DateTime? RequestTimeStamp { get; set; }

        [StringLength(200)]
        public string ResponseContentType { get; set; }

        public string ResponseContentBody { get; set; }

        public int? ResponseStatusCode { get; set; }

        [StringLength(500)]
        public string ResponseHeaders { get; set; }

        public DateTime? ResponseTimeStamp { get; set; }
    }
}
