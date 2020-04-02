using Domain.Models;

namespace Domain.Models.Communication
{
    public class CategoryResponse : BaseResponse<Category>
    {
        public CategoryResponse(Category category) : base(category) { }

        public CategoryResponse(string message) : base(message) { }
    }
}