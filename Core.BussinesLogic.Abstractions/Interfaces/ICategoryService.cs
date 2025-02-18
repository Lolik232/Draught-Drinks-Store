using Core.BussinesLogic.Abstractions.Primitives;
using DAL.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BussinesLogic.Abstractions.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<ICollection<int>>> AllSubcategoriesIdsAsync(string name);
        Task<Result<ICollection<int>>> AllSubcategoriesIdsAsync(int id);

        Task<Result<ICollection<Category>>> GetAllAsync();
    }
}
