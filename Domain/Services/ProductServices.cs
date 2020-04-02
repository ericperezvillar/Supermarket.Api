using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositories;
using Domain.Models.Communication;
using Resources;

namespace Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
            this._categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Product>> ListAsync()
        { 
            return await _productRepository.ListProductAsync();
        }

        public async Task<ProductResponse> SaveAsync(Product product)
        {
            try
            {
                var validateCategory = await ValidateCategoryById(product.CategoryId);
                if (validateCategory != null)
                {
                    return validateCategory;
                }

                var validateProduct = await ValidateProductExistsByName(product.Name);
                if (validateProduct != null)
                {
                    return validateProduct;
                }

                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(product);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when saving the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> UpdateAsync(int id, Product product)
        {
            try
            {
                var validateCategory = await ValidateCategoryById(product.CategoryId);
                if (validateCategory != null)
                {
                    return validateCategory;
                }

                var productResponse = await ValidateProductExistsById(id);
                if (!productResponse.Success)
                {
                    return productResponse;
                }

                productResponse.Resource.Name = product.Name;
                productResponse.Resource.UnitOfMeasurement = product.UnitOfMeasurement;
                productResponse.Resource.QuantityInPackage = product.QuantityInPackage;
                productResponse.Resource.CategoryId = product.CategoryId;

                _productRepository.Update(productResponse.Resource);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(productResponse.Resource);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            try
            {
                var productResponse = await ValidateProductExistsById(id);
                if (!productResponse.Success)
                {
                    return productResponse;
                }

                _productRepository.Delete(productResponse.Resource);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(productResponse.Resource);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            }
        }

        private async Task<ProductResponse> ValidateCategoryById(int id)
        {
            var existingCategory = await _categoryRepository.FindByIdAsync(id);

            if (existingCategory == null)
                return new ProductResponse("Category not found");

            return null;
        }

        private async Task<ProductResponse> ValidateProductExistsById(int id)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product not found");

            return new ProductResponse(existingProduct);
        }

        private async Task<ProductResponse> ValidateProductExistsByName(string name)
        {
            var existingProduct = await _productRepository.FindProductByName(name);

            if (existingProduct != null)
                return new ProductResponse("Product already exist");

            return null;
        }
    }
}