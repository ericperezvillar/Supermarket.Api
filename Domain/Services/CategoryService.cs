using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositories;
using Domain.Models.Communication;
using Serilog;

namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            try
            {
                return await _categoryRepository.ListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryResponse> SaveAsync(Category category)
        {
            try
            {
                var categoryResponse = await ValidateCategoryByName(category);
                if (categoryResponse != null)
                {
                    return categoryResponse;
                }

                await _categoryRepository.AddAsync(category);
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(category);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new CategoryResponse($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse> UpdateAsync(int id, Category category)
        {
            try
            {
                var categoryResponse = await ValidateCategoryById(id);
                if (!categoryResponse.Success)
                {
                    return categoryResponse;
                }

                categoryResponse.Resource.Name = category.Name;

                _categoryRepository.Update(categoryResponse.Resource);
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(categoryResponse.Resource);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new CategoryResponse($"An error occurred when updating the category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse> DeleteAsync(int id)
        {
            try
            {
                var categoryResponse = await ValidateCategoryById(id);
                if (!categoryResponse.Success)
                {
                    return categoryResponse;
                }

                _categoryRepository.Delete(categoryResponse.Resource);
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(categoryResponse.Resource);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new CategoryResponse($"An error occurred when updating the category: {ex.Message}");
            }
        }

        private async Task<CategoryResponse> ValidateCategoryByName(Category category)
        {
            var categoryValidation = await _categoryRepository.FindCategoryByName(category.Name);
            if (categoryValidation != null)
            {
                return new CategoryResponse("Category already exists");
            }
            return null;
        }

        private async Task<CategoryResponse> ValidateCategoryById(int id)
        {
            var existingCategory = await _categoryRepository.FindByIdAsync(id);

            if (existingCategory == null)
                return new CategoryResponse("Category not found");

            return new CategoryResponse(existingCategory);
        }
    }
}