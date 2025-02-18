using Authorization.V1;
using Core.BussinesLogic.Abstractions.Interfaces;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Draught_Drinks_Store_GRPC.Services.v1
{
    public class UserService : Authorization.V1.Authorization.AuthorizationBase
    {
        private readonly IUserService _userService;

        public UserService(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task<LoginResponce> Login(LoginRequest request, ServerCallContext context)
        {
            var result = _userService.CheckPassword(request.Login, request.Password);

            if (!result)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, ""));
            }

            var userResult = await _userService.GetByEmailAsync(request.Login);

            var token = JwtTokenValidator.GenerateToken(userResult.Value.Id.ToString(), userResult.Value.Role);

            var loginResponce = new LoginResponce
            {
                Token = token,
                Lifetime = (int)TimeSpan.FromMinutes(300).TotalSeconds,
                User = new LoginResponce.Types.User
                {
                    Email = userResult.Value.Email,
                    PhoneNumber = userResult.Value.ContactData.PhoneNumber
                }
            };

            return loginResponce;
        }

        public override Task<RegisterResponce> Register(RegisterRequest request, ServerCallContext context)
        {
            var result = _userService.Register(new Core.BussinesLogic.Abstractions.Entities.User.UserCreate(
                 request.User.Email,
                request.User.Password,
                new Core.BussinesLogic.Abstractions.Entities.User.ContactDataCreate(request.User.PhoneNumber)));

            if (result.IsFailture)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ""));
            }

            var responce = new RegisterResponce
            {
                Message = "Ok"
            };

            return Task.FromResult(responce);
        }
    }
}
