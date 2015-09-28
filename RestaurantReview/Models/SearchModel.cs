using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Models
{
    public class SearchModel<T>
    {
        // Search options
        public virtual int NumReturned { get; set; }
        public virtual int PageNum { get; set; }
        public virtual string OrderBy { get; set; }
        public virtual string Order { get; set; }

        public IQueryable<T> GetPage(IQueryable<T> queryObj)
        {
            if (this.NumReturned > 0)
            {
                // A client is expected to index page numbers by 1, but we'll index by 0 here
                this.PageNum = this.PageNum <= 0 ? 0 : this.PageNum - 1;
                queryObj = queryObj.Skip(this.PageNum * this.NumReturned).Take(this.NumReturned);
            }
            return queryObj;
        }
    }
}