using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ToDoList.Application.Common.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogInformation("[START] Runned {RequestName}", requestName);

            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();

            logger.LogInformation("[END] Finished {RequestName} in {ElapsedMilliseconds} ms",
                requestName, timer.ElapsedMilliseconds);

            return response;
        }
    }
}
