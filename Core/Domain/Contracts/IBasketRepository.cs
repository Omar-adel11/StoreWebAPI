using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Basket;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string Id);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? time);
        Task<bool> DeleteBasketAsync(string Id);
    }
}
