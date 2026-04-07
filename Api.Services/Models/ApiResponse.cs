using System.Net;

namespace Api.Services.Models
{
    /// <summary>
    /// Represents a generic API response.
    /// Contains status code, deserialized data, raw response content, and success indicator.
    /// </summary
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Content { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}