namespace HRManagement.Application.DTOs
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
