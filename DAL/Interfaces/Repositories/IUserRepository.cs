using DAL.EFCore.PostgreSQL;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IUserRepository : IBaseGetFiltered<User, int>
    { 
    }
}
