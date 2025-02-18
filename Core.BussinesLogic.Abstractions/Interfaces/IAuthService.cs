namespace Core.BussinesLogic.Abstractions.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(int userId, string password);
        Task<string> Authenticate(string login, string password);
    }
}
