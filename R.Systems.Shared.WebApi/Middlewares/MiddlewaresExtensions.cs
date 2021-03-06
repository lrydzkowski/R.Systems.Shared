using Microsoft.AspNetCore.Builder;

namespace R.Systems.Shared.WebApi.Middlewares;

public static class MiddlewaresExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
