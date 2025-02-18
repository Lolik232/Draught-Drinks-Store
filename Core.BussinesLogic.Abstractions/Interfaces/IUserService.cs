using Core.BussinesLogic.Abstractions.Entities.User;
using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;

namespace Core.BussinesLogic.Abstractions.Interfaces;

public interface IUserService
{
    Result<int> Register(UserCreate user);


    Task<Result<User>> GetByEmailAsync(string email);

    bool CheckPassword(int id, string password);
    bool CheckPassword(string login, string password);
}
