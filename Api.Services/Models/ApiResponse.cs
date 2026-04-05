using System.Net;

namespace API.Services.Models
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Content { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}