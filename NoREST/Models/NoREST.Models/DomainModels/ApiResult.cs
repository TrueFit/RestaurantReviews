using System.Net;

namespace NoREST.Models.DomainModels
{
    public class ApiResult<T>
    {
        public ApiResult(T result, bool isSuccess = true, string errorMessage = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Value = result;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public T Value { get; }
        public string ErrorMessage { get; }
        public bool IsSuccess { get; }
        public HttpStatusCode StatusCode { get; }
    }
}
