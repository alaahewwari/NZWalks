using System.Net;
using System.Net.Mime;

namespace NZWalks.API.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger
            ,RequestDelegate next) {
            this.logger = logger;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId= Guid.NewGuid();
                //log this exception
                logger.LogError(ex,$"{errorId} : {ex.Message}");
                //return custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong. Please contact support."
                };
                await httpContext.Response.WriteAsJsonAsync(error);

            }
        }
    }
}
