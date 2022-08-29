using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Models.Interfaces
{
    public interface IEntity<T> : IAuditEntity
    {
        T Id { get; set; }
    }
}
