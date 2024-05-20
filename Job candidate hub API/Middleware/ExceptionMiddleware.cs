using System.Net;
using System.Text.Json;
using Job_candidate_hub_API.Errors;

namespace Job_candidate_hub_API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = ex switch
                {
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    NotFoundException   => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };
                
                _logger.LogError(ex, ex.Message);

                var errorResponse = _env.IsDevelopment()
                    ? new ApiException(response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(response.StatusCode, ex.Message ,"Internal Server Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(errorResponse, options);

                await response.WriteAsync(json);
            }
        }
    }
}