using DAL.EFCore.PostgreSQL;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IContactDataRepository : IBaseGetFiltered<ContactData, int>
    {

    }
}
