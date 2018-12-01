using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BudgetUnderControl.API.Framework
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Logger.Error(exception, string.Format("{0} Request: {2}{3} | StackTrace: {1}", exception.Message, exception.StackTrace, context.Request.Path.ToString(), context.Request.QueryString.ToString()));
            var errorCode = "error";
            var statusCode = HttpStatusCode.InternalServerError;
            var exceptionType = exception.GetType();

            var response = new { code = errorCode, message = exception.Message };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(payload);
        }
    }
}
