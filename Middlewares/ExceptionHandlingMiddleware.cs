using System.Net;
using app2.Dtos.Common;

namespace app2.Middlewares
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
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 记录详细异常到日志，但不返回给客户端
                _logger.LogError(ex, "Unhandled exception. Path: {Path}, TraceId: {TraceId}", context.Request.Path, context.TraceIdentifier);

                // 统一错误响应，不泄露栈信息
                var (statusCode, msg) = MapStatusAndMessage(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = ApiResponse<object>.Fail(msg, statusCode);
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static (int statusCode, string message) MapStatusAndMessage(Exception ex)
        {
            // 可按需扩展更多异常类型映射
            return ex switch
            {
                BadHttpRequestException => (StatusCodes.Status400BadRequest, "请求不合法"),
                ArgumentException => (StatusCodes.Status400BadRequest, "参数错误"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "资源不存在"),
                _ => (StatusCodes.Status500InternalServerError, "服务器内部错误")
            };
        }
    }
}