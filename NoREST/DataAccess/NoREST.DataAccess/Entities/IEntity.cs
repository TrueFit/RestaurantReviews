using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.DataAccess.Entities
{
    public interface IEntity<T>
    {
        [Key]
        T Id { get; set; }
    }
}
