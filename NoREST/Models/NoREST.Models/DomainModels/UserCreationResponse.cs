using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoREST.Models.DomainModels
{
    public class UserCreationResponse
    {
        public string Error { get; set; }
        public bool IsSuccess { get; set; }
        public string IdentityProviderId { get; set; }
    }
}
