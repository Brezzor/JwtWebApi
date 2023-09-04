using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Cryptography.Xml;

namespace JwtWebApi.Filters
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorize = context.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .Any() || context.MethodInfo.DeclaringType!.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

            if (authorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme 
                            { 
                                Reference = new OpenApiReference 
                                { 
                                    Type = ReferenceType.SecurityScheme, 
                                    Id = "Bearer" 
                                } 
                            },
                            new string[] {}
                        }
                    }
                };
            }
        }
    }
}
