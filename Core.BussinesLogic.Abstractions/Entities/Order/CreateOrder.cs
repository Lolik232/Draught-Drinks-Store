using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BussinesLogic.Abstractions.Entities.Order
{
    public record ProductCount(int ProductId, int Count);
    public record CreateOrder(ICollection<ProductCount> ProductsInOrder);
}
