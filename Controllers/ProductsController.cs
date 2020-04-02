using Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Resources;
using Domain.Models;
using Serilog;
using System;
using Extensions;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Product list
        /// </summary>
        /// <remarks>
        /// Get list of product and its category
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResource>), 200)]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var products = await _productService.ListAsync();
                var resources = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(products);
                Log.Logger.Information("Return list of Products");
                return Ok(resources);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to get list of Products. Error: {error}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add a new Product
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveProductResource resource)
        {
            try
            {
                Log.Logger.Information("Add new product {@resource} was executed", resource);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState.GetErrorMessages());

                var product = _mapper.Map<SaveProductResource, Product>(resource);
                var result = await _productService.SaveAsync(product);

                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                var productResource = _mapper.Map<Product, ProductResource>(result.Resource);

                Log.Logger.Information("Product {@resource} added successfully", productResource);

                return Ok(productResource);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to add new product: {@resource}. Error: {error}", resource, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update product details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveProductResource resource)
        {
            try
            {
                Log.Logger.Information("Update product {@resource} was executed", resource);

                var product = _mapper.Map<SaveProductResource, Product>(resource);
                var result = await _productService.UpdateAsync(id, product);

                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                var productResource = _mapper.Map<Product, ProductResource>(result.Resource);

                Log.Logger.Information("Product {@resource} updated successfully", productResource);

                return Ok(productResource);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to update product: {@resource}. Error: {error}", resource, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                Log.Logger.Information("Delete product #{id} was executed", id);

                var result = await _productService.DeleteAsync(id);

                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                var productResource = _mapper.Map<Product, ProductResource>(result.Resource);

                Log.Logger.Information("Product {productName} updated successfully", productResource);

                return Ok(productResource);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to delete product #{id}. Error: {error}", id, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}