using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace NoREST.Api.Auth
{
    public class JwtModel
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Kid { get; set; }
        public DateTime ValidTo { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        public JwtModel() { }

        public JwtModel(JsonWebToken token)
        {
            Issuer = token.Issuer;
            Subject = token.Subject;
            Kid = token.Kid;
            ValidTo = token.ValidTo;
            Claims = token.Claims;
        }
    }
}

