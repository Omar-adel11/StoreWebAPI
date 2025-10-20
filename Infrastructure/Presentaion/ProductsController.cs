using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions.Products;

namespace Presentaion
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductsController(IProductService _productService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductAsync();
            if(products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        [HttpGet("Brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandsAsync();
            if (brands == null)
            {
                return NotFound("No brands found.");
            }
            return Ok(brands);
        }
        [HttpGet("Types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _productService.GetAllTypesAsync();
            if (types == null)
            {
                return NotFound("No types found.");
            }
            return Ok(types);
        }
    }
}
