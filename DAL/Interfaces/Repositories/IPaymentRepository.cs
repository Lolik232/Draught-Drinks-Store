using DAL.Abstractions.Entities;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IPaymentRepository : IBaseGetFiltered<DAL.Abstractions.Entities.Payment, int>
    {
        Task<int> CreateAsync(DAL.Abstractions.Entities.Payment payment);
    }
}
