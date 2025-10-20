using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using ServicesAbstractions;
using ServicesAbstractions.Products;

namespace Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper) : IServiceManager
    {
        public IProductService ProductService { get; } = new Products.ProductService(_unitOfWork, _mapper);
    }
}
