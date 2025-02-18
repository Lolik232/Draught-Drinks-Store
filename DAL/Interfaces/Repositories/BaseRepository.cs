using System.Linq.Expressions;

namespace DAL.Abstractions.Interfaces.Repositories
{
    public interface IBaseGetRepository<T, TId> where T : class
    {
        T Get(TId id);
        Task<T> GetAsync(TId id);
    }

    public interface IBaseGetFiltered<T, TId> : IBaseGetRepository<T, TId> where T : class
    {
        T Get(TId id, List<string> include);
        Task<T> GetAsync(TId id, List<string> include);

        T Get(Expression<Func<T, bool>> expression);
        T Get(Expression<Func<T, bool>> expression, List<string> include);

        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string> include);


        public interface WithGetAll : IBaseGetFiltered<T, TId>
        {
            IEnumerable<T> GetAll(int offset, int count);
            Task<IEnumerable<T>> GetAllAsync(int offset, int count);

            IEnumerable<T> GetAll(int offset, int count, Expression<Func<T, bool>> expression, string sorting, List<string> include);
            Task<IEnumerable<T>> GetAllAsync(int offset, int count, Expression<Func<T, bool>> expression);
        }
    }

    public interface IBaseRemoveRepository<T, TId> where T : class
    {
        bool Remove(TId id);
        public interface WithRemoveAll : IBaseRemoveRepository<T, TId>
        {
            bool RemoveAll();
        }
    }
}
