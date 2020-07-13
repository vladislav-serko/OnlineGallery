using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using OnlineGallery.BLL.Exceptions;

namespace OnlineGallery.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly IActionResultExecutor<ObjectResult> _executor;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next, IActionResultExecutor<ObjectResult> executor)
        {
            _next = next;
            _executor = executor;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            var problemDetails = CreateProblemDetails(context, ex);

            var routeData = context.GetRouteData();
            var actionContext = new ActionContext(context, routeData, new ActionDescriptor());
            var result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };

            return _executor.ExecuteAsync(actionContext, result);
        }

        private ProblemDetails CreateProblemDetails(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Title = "Internal server error.",
                Detail = ex.Message
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            switch (ex)
            {
                case IdentityException e:
                    problemDetails.Title = "One or more identity errors occured.";
                    problemDetails.Detail = "Please refer to the errors property for additional details.";
                    problemDetails.Extensions.Add("errors", e.Errors);
                    code = HttpStatusCode.BadRequest;
                    break;
                case ObjectNotFoundException _:
                    problemDetails.Title = "Not found.";
                    code = HttpStatusCode.NotFound;
                    break;
                case ImageAccessException _:
                    problemDetails.Title = "You do not have access to this action.";
                    code = HttpStatusCode.Forbidden;
                    break;
                case UserIdMismatchException e:
                    problemDetails.Title = "User id does not match current user id.";
                    problemDetails.Extensions.Add("providedId", e.ProvidedId);
                    problemDetails.Extensions.Add("currentUserId", e.CurrentUserId);
                    code = HttpStatusCode.BadRequest;
                    break;
                case InvalidOperationException _:
                    code = HttpStatusCode.BadRequest;
                    problemDetails.Title = "This operation is invalid.";
                    break;
            }

            problemDetails.Status = (int) code;
            problemDetails.Type = $"https://httpstatuses.com/{(int) code}";
            return problemDetails;
        }
    }
}