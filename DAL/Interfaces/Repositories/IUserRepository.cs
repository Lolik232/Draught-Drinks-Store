using DAL.Abstractions.Entities;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IUserRepository : IBaseGetFiltered<User, int>
    {
        long CreateUser(User user);
        Task<long> CreateUserAsync(User user);
    }
}
