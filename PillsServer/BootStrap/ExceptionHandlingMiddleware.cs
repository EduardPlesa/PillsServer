using FluentValidation;
using PillsServer.Common;
using System.Net;
using System.Text;

namespace MediConnect.BootStrap
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine(context.Request.Path);
            Console.WriteLine();
            context.Request.EnableBuffering();
            using(var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                Console.WriteLine(body);
                context.Request.Body.Position = 0;
            }
            Console.WriteLine();
            try
            {
                await _next(context);
            } catch(Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Exception:" + ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            Console.WriteLine("---------------------------------");
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if(exception is ValidationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                var response = new BaseResponse
                {
                    Ok = false,
                    Errors = ((ValidationException)exception).Errors.Select(e => e.ErrorMessage).ToList()
                };
                return context.Response.WriteAsJsonAsync(response);
            } else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please try again later.",
                    Detailed = exception.Message // You might want to remove this in production
                };
                return context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}