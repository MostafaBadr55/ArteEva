using ArtEva.Services.Implementation;

namespace ArtEva.Controllers.Filters.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteError(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await WriteError(context, StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (NotValidException ex)
            {
                await WriteError(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteError(context, StatusCodes.Status500InternalServerError,
                    "Unexpected error occurred.");
            }
        }

        private async Task WriteError(HttpContext context, int statusCode, string message)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new { error = message });
            }
            else
            {
                // If headers already sent, you MUST not modify response
                // You can only log this case.
                Console.WriteLine("Warning: Cannot write error response because headers already sent.");
            }
        }
    }

}
