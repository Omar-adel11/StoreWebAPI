using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Order;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Specifications;
using ServicesAbstractions.Payment;
using Shared.DTOs.Basket;
using Stripe;

namespace Services.Payments
{
    public class PaymentService(IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IConfiguration configuration, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreatePaymentIntentAsync(string basketId)
        {
            //Calculate Total Amount including shipping 

            //get basket
            
            var basket = await _basketRepository.GetBasketAsync(basketId);
            
            //check that prices are correct of items in basket

            foreach(var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<int, Domain.Entities.Product.Product>().GetByIdAsync(item.Id);
                if(product is null) throw new Exception($"Product with id {item.Id} not found");
                if(item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
            }

            var amount = basket.Items.Sum(i => i.Price * i.Quantity); 

            //check if delivery method exists and get shipping price

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.GetRepository<int, DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                if(deliveryMethod is null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
                //set shipping price
                basket.ShippingPrice = deliveryMethod.Price;
            }
            else
            {
                throw new DeliveryMethodNotFoundException(-1);
            }

            //calculate total amount

            var totalAmount = amount + basket.ShippingPrice.Value;  

            //Send the amount to stripe and create a payment intent

            StripeConfiguration.ApiKey = configuration["StripeOptions:SecretKey"];

            PaymentIntentService PaymentIntenetService = new PaymentIntentService();

            var paymentIntent = new PaymentIntent();


            if (basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(totalAmount * 100), //amount in cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                };

                paymentIntent = await PaymentIntenetService.CreateAsync(options);

            }
            else
            {

                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(totalAmount * 100), //amount in cents
          
                };

                paymentIntent = await PaymentIntenetService.UpdateAsync(basket.PaymentIntentId, options);
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            var Basket = await _basketRepository.UpdateBasketAsync(basket,TimeSpan.FromDays(1));

            return _mapper.Map<BasketDto>(Basket);  


        }

        public async Task UpdatePaymentStatusAsync(string json, string signatureHeader)
        {
             string endpointSecret = configuration.GetSection("StripeOptions")["endpointSecret"];
            
                var stripeEvent = EventUtility.ParseEvent(json,throwOnApiVersionMismatch:false);

                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret,throwOnApiVersionMismatch:false);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                await UpdatePaymentStatusAsyncSucceeded(paymentIntent.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {

                await UpdatePaymentStatusAsyncFailed(paymentIntent.Id);
                }
                 // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
             
            
        }

        private async Task UpdatePaymentStatusAsyncFailed(string id)
        {
            var order = await _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().GetByIdAsync(new OrderByPaymentIntentIdSpecification(id));
            if(order is not null)
            {
                order.Status = OrderStatus.PaymentFailed;
                    _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task UpdatePaymentStatusAsyncSucceeded(string id)
        {
            var order = await _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().GetByIdAsync(new OrderByPaymentIntentIdSpecification(id));
            if (order is not null)
            {
                order.Status = OrderStatus.PaymentSuccess;
                _unitOfWork.GetRepository<Guid, Domain.Entities.Order.Order>().Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
