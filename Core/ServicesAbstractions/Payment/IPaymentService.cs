using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOs.Basket;

namespace ServicesAbstractions.Payment
{
    public interface IPaymentService
    {
        Task<BasketDto> CreatePaymentIntentAsync(string basketId);
        Task UpdatePaymentStatusAsync(string json, string signatureHeader);
    }
}
