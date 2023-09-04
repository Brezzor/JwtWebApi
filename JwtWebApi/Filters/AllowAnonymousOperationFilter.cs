using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JwtWebApi.Filters
{
    public class AllowAnonymousOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowAnonymous = context.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                .Any() || context.MethodInfo.DeclaringType!.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

            if (allowAnonymous)
            {
                operation.Security = null;
            }
        }
    }
}
