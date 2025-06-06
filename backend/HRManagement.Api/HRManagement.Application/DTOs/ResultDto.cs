using System.Net;
using System.Text.Json.Serialization;

namespace HRManagement.Application.DTOs
{
    public class ResultDto<T>
    {
        [JsonIgnore] // Ensure not return in API Response
        public bool IsSuccess { get; private set; }

        [JsonIgnore] // Ensure not return in API Response
        public HttpStatusCode StatusCode { get; private set; }

        [JsonInclude] // Ensure return in API Response (because of private set)
        public string? ErrorMessage { get; private set; }

        [JsonInclude] // Ensure return in API Response (because of private set)
        public T? Data { get; private set; }

        public static ResultDto<T> Success(T data, HttpStatusCode statusCode) => new() { IsSuccess = true, StatusCode = statusCode, Data = data };
        public static ResultDto<T> Failure(string error, HttpStatusCode statusCode) => new() { IsSuccess = false, StatusCode = statusCode, ErrorMessage = error };
    }
}
