using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using System.Net;
using System.Text.Json;

namespace StoreManagement.API.Common.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex.ErrorCode.Message, "Business exception occurred");
                await HandleAppExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleUnhandledExceptionAsync(context, ex);
            }
        }

        private static Task HandleAppExceptionAsync(HttpContext context, AppException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.ErrorCode.StatusCode;

            var response = new ApiResponse<object>
            {
                Success = ex.ErrorCode.Success,
                Message = ex.ErrorCode.Message,
                StatusCode = ex.ErrorCode.StatusCode,
                
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleUnhandledExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "Internal server error",
                StatusCode = 500
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
