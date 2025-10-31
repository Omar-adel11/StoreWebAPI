using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Basket;
using Domain.Exceptions;
using ServicesAbstractions.Baskets;
using Shared.DTOs.Basket;

namespace Services.Basket
{
    public class BasketService(IBasketRepository basketRepository,IMapper mapper) : IBasketService
    {
        public async Task<BasketDto> GetBasketAsync(string Id)
        {
            var basket = await basketRepository.GetBasketAsync(Id);
            if (basket is null) throw new basketNotFoundException(Id);
            var result =  mapper.Map<BasketDto>(basket);
            return result;
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);
            basket = await basketRepository.UpdateBasketAsync(basket,null);
            if (basket is null) throw new BasketCreateOrUpdateBadRequestException();
            var result = mapper.Map<BasketDto>(basket);
            return result;
        }
        public async Task<bool> DeleteBasketAsync(string Id)
        {
            var flag = await basketRepository.DeleteBasketAsync(Id);    
            if(flag == false) throw new BasketDeleteBadRequestException();
            return flag;
        }

       
    }
}
