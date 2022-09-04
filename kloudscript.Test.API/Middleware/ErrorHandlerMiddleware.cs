using kloudscript.Test.API.Entity;
using System.Net;
using System.Text.Json;

namespace kloudscript.Test.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                ResponseEntity responseModel = new ResponseEntity();
                switch (error)
                {
                    case AppException e: 
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.statusCode = HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e: 
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.statusCode =  HttpStatusCode.NotFound;
                        break;
                    default: 
                        _logger.LogError(error, error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.statusCode =  HttpStatusCode.InternalServerError;
                        break;
                }
                responseModel.message = error?.Message;
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
