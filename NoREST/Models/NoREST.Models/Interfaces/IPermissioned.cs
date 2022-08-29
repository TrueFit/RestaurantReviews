namespace NoREST.Models.Interfaces
{
    public interface IPermissioned
    {
        bool IsAdmin { get; set; }
    }
}
