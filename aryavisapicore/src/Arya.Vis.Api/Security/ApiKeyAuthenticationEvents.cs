using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public class ApiKeyAuthenticationEvents
    {
        public Func<ApiKeyAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

        public Func<ValidateCredentialsContext, IServiceProvider, Task> OnValidateCredentials { get; set; } = (context, serviceProvider) => Task.CompletedTask;

        public virtual Task AuthenticationFailed(ApiKeyAuthenticationFailedContext context) => OnAuthenticationFailed(context);

        public virtual Task ValidateCredentials(ValidateCredentialsContext context, IServiceProvider serviceProvider) => OnValidateCredentials(context, serviceProvider);
    }
}
