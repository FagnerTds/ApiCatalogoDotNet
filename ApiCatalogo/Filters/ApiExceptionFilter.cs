﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalogo.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
   
    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Ocorreu um erro não tratado: Status Code 500");
        context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua Solicitação: Status Code 500")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}
