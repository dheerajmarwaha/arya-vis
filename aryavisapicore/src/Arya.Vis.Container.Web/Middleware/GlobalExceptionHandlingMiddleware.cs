using Arya.Exceptions;
using Arya.Vis.Container.Web.Enums;
using Arya.Vis.Container.Web.Exceptions;
using Arya.Vis.Container.Web.Models;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Exceptions;
using Arya.Vis.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly static string UnknownError = "UNKNOWN_ERROR";

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ILogger<GlobalExceptionHandlingMiddleware> logger, IUserService userService)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger, userService);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<GlobalExceptionHandlingMiddleware> logger, IUserService userService)
        {
            var code = HttpStatusCode.InternalServerError;
            string appCode = UnknownError;
            object errorInfo = null;
            if (exception is EntityNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                var notFoundException = exception as EntityNotFoundException;
                appCode = $"{notFoundException.EntityName.ToUpper()}_NOT_FOUND";
            }
            else if (exception is InvalidArgumentException)
            {
                code = HttpStatusCode.BadRequest;
                var argumentException = exception as InvalidArgumentException;
                appCode = $"INVALID_{argumentException.ParamName.ToUpper()}";
            }
            else if (exception is UnauthorizedEntityAccessException)
            {
                code = HttpStatusCode.Forbidden;
                var forbiddenException = exception as UnauthorizedEntityAccessException;
                appCode = $"{forbiddenException.EntityName.ToUpper()}_NOT_ACCESSIBLE";
            }
            else if (exception is ServiceUnavailableException)
            {
                code = HttpStatusCode.ServiceUnavailable;
                var unavailableException = exception as ServiceUnavailableException;
                appCode = $"{unavailableException.ServiceName.ToUpper()}_IS_UNAVAILABLE";
            }
            else if (exception is NotSupportedException)
            {
                code = HttpStatusCode.NotAcceptable;
                appCode = "UNSUPPORTED_OPERATION";
            }
            else if (exception is GatewayException)
            {
                code = HttpStatusCode.InternalServerError;
                var gatewayException = exception as GatewayException;
                appCode = "GATEWAY_ERROR";
            }
            else if (exception is RepositoryException)
            {
                code = HttpStatusCode.InternalServerError;
                var repositoryException = exception as RepositoryException;
                appCode = string.IsNullOrEmpty(repositoryException.RepositoryName) ? "UNKNOWN_REPOSITORY_ERROR" :
                    $"{repositoryException.RepositoryName.ToUpper()}_REPOSITORY_ERROR";
            }
            else if (exception is UserNotFoundException)
            {
                code = HttpStatusCode.Unauthorized;
                appCode = "USER_NOT_FOUND";
            }
            else if (exception is NotImplementedException)
            {
                code = HttpStatusCode.NotImplemented;
                appCode = "NOT_IMPLEMENTED";
            }
            else if (exception is UnauthorizedOperationException)
            {
                code = HttpStatusCode.Forbidden;
                appCode = "UNAUTHORIZED_OPERATION";
            }
            else if (exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Forbidden;
                appCode = "UNAUTHORIZED_ACCESS";
            }
            //else if (exception is SourceAuthorizationFailedException)
            //{
            //    code = HttpStatusCode.Forbidden;
            //    var sourceAuthorizationFailedException = exception as SourceAuthorizationFailedException;
            //    appCode = "SOURCE_FORBIDDEN";
            //    errorInfo = sourceAuthorizationFailedException.SourceInfo;
            //}
            //else if (exception is SourceAuthenticationFailedException)
            //{
            //    code = HttpStatusCode.Unauthorized;
            //    var sourceAuthenticationFailedException = exception as SourceAuthenticationFailedException;
            //    appCode = "INVALID_SOURCE_CREDENTIALS";
            //    errorInfo = sourceAuthenticationFailedException.SourceInfo;
            //}
            else if (exception is CountryNotSupportedException)
            {
                appCode = AppCode.UnsupportedCountry.ToString();
            }
            else if (exception is TagTypeNotSupportedException)
            {
                appCode = AppCode.UnsupportedTagType.ToString();
            }
            else if (exception is FeatureNotSupportedException)
            {
                appCode = AppCode.UnsupportedFeature.ToString();
            }
            else if (exception is LimitExceededException)
            {
                code = HttpStatusCode.Forbidden;
                var limitExceededException = exception as LimitExceededException;
                appCode = $"{limitExceededException.Action.ToUpper()}_LIMIT_EXCEEDED";
            }
            else if (exception is JobNotSupportedException)
            {
                code = (HttpStatusCode)422;
                appCode = "JOB_NOT_SUPPORTED";
            }
            else if (exception is InvalidOperationException)
            {
                code = (HttpStatusCode)406;
                appCode = "INVALID_OPERATION";
            }
            else if (exception is UnprocessableEntityException)
            {
                code = (HttpStatusCode)422;
                appCode = $"UNPROCESSABLE_{(exception as UnprocessableEntityException).EntityName}";
            }
            //else if (exception is SourceCredentialsNotConfiguredException)
            //{
            //    var credentialsNotConfiguredException = exception as SourceCredentialsNotConfiguredException;
            //    if (string.IsNullOrWhiteSpace(credentialsNotConfiguredException.Reason))
            //    {
            //        code = HttpStatusCode.NotFound;
            //        appCode = "CREDENTIALS_NOT_FOUND";
            //        errorInfo = credentialsNotConfiguredException.SourceInfo;
            //    }
            //    else
            //    {
            //        code = HttpStatusCode.Forbidden;
            //        appCode = "CREDENTIALS_NOT_SUPPORTED";
            //        errorInfo = credentialsNotConfiguredException.SourceInfo;
            //    }
            //}
            else if (exception is ConflictingEntityException)
            {
                var conflictingEntityException = exception as ConflictingEntityException;
                code = (HttpStatusCode)409;
                appCode = $"CONFLICTING_{conflictingEntityException.EntityName.ToUpper()}";
            }
            //else if (exception is SourceCreditExhaustedException)
            //{
            //    var creditExhaustedException = exception as SourceCreditExhaustedException;
            //    code = (HttpStatusCode)402;
            //    appCode = "SOURCE_CREDIT_EXHAUSTED";
            //    errorInfo = creditExhaustedException.SourceInfo;
            //}
            //else if (exception is SourceRequestBlockedException)
            //{
            //    var sourceRequestBlockedException = exception as SourceRequestBlockedException;
            //    code = HttpStatusCode.Forbidden;
            //    appCode = "SOURCE_REQUEST_FAILED";
            //    errorInfo = sourceRequestBlockedException.SourceInfo;
            //}
            //else if (exception is SourceCandidateRemovedException)
            //{
            //    var sourceCandidateRemovedException = exception as SourceCandidateRemovedException;
            //    code = (HttpStatusCode)404;
            //    appCode = "SOURCE_CANDIDATE_REMOVED";
            //    errorInfo = sourceCandidateRemovedException.SourceInfo;
            //}
            //else if (exception is SourceCandidateResumeUpdatedException)
            //{
            //    var sourceCandidateResumeUpdatedException = exception as SourceCandidateResumeUpdatedException;
            //    code = (HttpStatusCode)404;
            //    appCode = "SOURCE_CANDIDATE_RESUME_UPDATED";
            //    errorInfo = sourceCandidateResumeUpdatedException.SourceInfo;
            //}
            //else if (exception is SourceServiceUnavailableException)
            //{
            //    var sourceCandidateResumeUpdatedException = exception as SourceCandidateResumeUpdatedException;
            //    code = (HttpStatusCode)503;
            //    appCode = "SOURCE_UNAVAILABLE_AT_THE_MOMENT";
            //    errorInfo = sourceCandidateResumeUpdatedException.SourceInfo;
            //}
            else if (exception is PaymentRequiredException)
            {
                code = (HttpStatusCode)402;
                appCode = "ARYA_CREDIT_EXHAUSTED";
            }
            else if (exception is IdentityProviderCodeConfirmationFailedException)
            {
                var ex = exception as IdentityProviderCodeConfirmationFailedException;
                code = HttpStatusCode.BadRequest;
                appCode = ex.Reason;
            }
            var isCurrentUser = userService.IsCurrentUserSet();
            User currentUser = null;
            if (isCurrentUser)
            {
                currentUser = userService.GetCurrentUser();
            }
            if (appCode == UnknownError)
            {
                logger.LogError(exception, "Exception handled by global exception handling middleware. AppCode {@AppCode} and user {@User}", appCode, currentUser);
            }
            else
            {
                logger.LogInformation(exception, "Exception handled by global exception handling middleware. AppCode {@AppCode} and user {@User}", appCode, currentUser);
            }
            var result = JsonConvert.SerializeObject(new ApiErrorResponse(exception.Message, appCode, errorInfo));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
