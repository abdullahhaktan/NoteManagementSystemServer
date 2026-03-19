namespace NoteManagemenSystemServer.Middleware
{
    // Global exception handler — catches unhandled exceptions across the entire pipeline
    // and returns a consistent JSON error response instead of the default HTML error page
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                // Thrown when a requested resource is not found (e.g. note not found by id)
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = 404,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors — never expose stack trace in production
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = 500,
                    message = "Sunucu hatası oluştu.",
                    detail = ex.Message
                });
            }
        }
    }
}