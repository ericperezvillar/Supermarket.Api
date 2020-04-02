namespace Domain.Models.Communication
{
    public abstract class BaseResponse<T>
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
        public T Resource { get; protected set; }

        public BaseResponse(string message)
        {
            Success = false;
            Message = message;
            Resource = this.Resource;
        }
        public BaseResponse(T resource)
        {
            Success = true;
            Message = null;
            Resource = resource;
        }
    }
}