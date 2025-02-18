using Core.BussinesLogic.Abstractions.Interfaces;

namespace Core.BussinesLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public Task<string> Authenticate(int userId, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> Authenticate(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
