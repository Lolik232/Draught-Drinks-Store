using Core.BussinesLogic.Abstractions.Entities.User;
using Core.BussinesLogic.Abstractions.Interfaces;
using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Core.BussinesLogic.Services
{
   

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private string Hash(string password)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("ssssssssssssssss"), 10000);
            return Convert.ToBase64String(pbkdf2.GetBytes(20));
        }

        private bool Verify(string password, string hash)
        {
            var hashToVerify = Hash(password);
            return hash == hashToVerify;
        }

        public bool CheckPassword(int id, string password)
        {
            var user = _userRepository.Get(id);
            return Verify(password, user.PasswordHash);
        }
         
        public bool CheckPassword(string login, string password)
        {
            var user = _userRepository.Get(u => u.Email == login || u.ContactData.PhoneNumber == login);
            if (user is null) return false;

            return Verify(password, user.PasswordHash);
        }

        public Result<int> Register(UserCreate user)
        {
            var foundedUser = _userRepository.Get(u => u.Email == user.Email);
            if (foundedUser is not null)
            {
                return Error.NullValue;
            }

            return (int)_userRepository.CreateUser(new DAL.Abstractions.Entities.User
            {
                Email = user.Email,
                EmailConfirmed = false,
                Role = "user",
                PasswordHash = Hash(user.Password),
                ContactData = new DAL.Abstractions.Entities.ContactData
                {
                    PhoneNumber = user.ContactData.PhoneNumber
                }
            });
        }

        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            var user = _userRepository.Get(u => u.Email == email, new() { nameof(ContactData) });

            if (user is null) return Error.NullValue;

            return user;
        }
    }
}
