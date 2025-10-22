using System.Text.Json.Serialization;

namespace StoreManagement.API.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success", int statusCode = 200)
            => new() { Success = true, Message = message, Data = data, StatusCode = statusCode };

        public static ApiResponse<T> Fail(List<string> errors, string message = "Error", int statusCode = 400)
            => new() { Success = false, Message = message, Errors = errors, StatusCode = statusCode };
    }
}
