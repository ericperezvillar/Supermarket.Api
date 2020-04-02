using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Domain.Services;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Serilog;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;   
            _mapper = mapper;
        }

        /// <summary>
        /// Category list
        /// </summary>
        /// <remarks>
        /// Get list of categories
        /// </remarks>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>),200)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var categories = await _categoryService.ListAsync();
                var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
                Log.Logger.Information("Return list of Categories");
                return Ok(resources);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to get list of Categories. Error: {error}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveCategoryResource resource)
        {
            try
            {
                Log.Logger.Information("Add new category {@resource} was executed", resource);
                
                var category = _mapper.Map<SaveCategoryResource, Category>(resource);
                var result = await _categoryService.SaveAsync(category);

                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);

                Log.Logger.Information("Category {@resource} added successfully", categoryResource);

                return Ok(categoryResource);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to add new category: {@resource}. Error: {error}", resource, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update category details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCategoryResource resource)
        {
            try
            {
                Log.Logger.Information("Update category {@resource} was executed", resource);

                var category = _mapper.Map<SaveCategoryResource, Category>(resource);
                var result = await _categoryService.UpdateAsync(id, category);

                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);

                Log.Logger.Information("Category {@resource} updated successfully", categoryResource);

                return Ok(categoryResource);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to update category: {@resource}. Error: {error}", resource, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CategoryResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                Log.Logger.Information("Delete category #{id} was executed", id);

                var result = await _categoryService.DeleteAsync(id);

                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);

                Log.Logger.Information("Category {categoryName} updated successfully", categoryResource);

                return Ok(categoryResource);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to delete category #{id}. Error: {error}", id, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}