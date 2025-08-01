﻿using ApiCatalogo.Model;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ApiCatalogo.Extentions;

public static class ApiExceptionMiddlewareExtentions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {                               
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "aplication/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }
}
