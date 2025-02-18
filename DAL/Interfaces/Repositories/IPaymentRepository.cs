namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IPaymentRepository : IBaseGetFiltered<DAL.EFCore.PostgreSQL.Payment, int>
    {
    }
}
