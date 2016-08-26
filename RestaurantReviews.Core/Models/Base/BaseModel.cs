using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Core
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            ValidationErrors = new List<string>();
            Created = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public bool IsValid { get { return this.ValidationErrors.Count() == 0; } }
        public List<string> ValidationErrors { get; internal set; }
        public abstract void Validate();
    }
}
