using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Domain.Exceptions;
using ServicesAbstractions;
using ServicesAbstractions.Orders;
using Shared.DTOs.Orders;
using Services.Specifications;

namespace Services.Order
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper,IBasketRepository _BasketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string UserEmail)
        {
            //map order address
            var OrderAdress = _mapper.Map<OrderAddress>(request.ShipToAddress);

            //Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetByIdAsync(request.DeliveryMethodId);
            if(deliveryMethod == null) throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);

            //Get Basket and its items from Basket Service
            var Basket = await _BasketRepository.GetBasketAsync(request.BasketId);
            if(Basket == null) throw new basketNotFoundException(request.BasketId);

            //map Basket items to Order Items
            var orderItems = new List<OrderItem>();
            foreach(var item in Basket.Items)
            {
                //check price
                var product = await _unitOfWork.GetRepository<int, Product>().GetByIdAsync(item.Id);
                if(product == null) throw new ProductNotFoundException(item.Id);
                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
                var ProductInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(ProductInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            //Calculate subtotal
            var subtotal = orderItems.Sum(i => i.Price * i.Quantity);

            // Change 'Order' to 'Domain.Entities.Order.Order' to resolve ambiguity between namespace and type
            var order = new Domain.Entities.Order.Order(UserEmail, OrderAdress, deliveryMethod, orderItems, subtotal);
            
            //Add Order 
            await _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().AddAsync(order);
            var count = await _unitOfWork.SaveChangesAsync();
            if (count <= 0) throw new CreateOrderBadRequestExcecption();

            //Map to OrderResponse DTO
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid Id, string UserEmail)
        {
            var spec = new OrderSpecifications(Id, UserEmail);
            var order = await _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().GetByIdAsync( spec);
            return _mapper.Map<OrderResponse?>(order);
        }

        public async  Task<IEnumerable<OrderResponse?>> GetOrdersForSpecificUserAsync(string UserEmail)
        {
            var spec = new OrderSpecifications(UserEmail);
            var order = await _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().GetAllAsync(spec);
            return  _mapper.Map<IEnumerable<OrderResponse?>>(order);
        }
    }
}
