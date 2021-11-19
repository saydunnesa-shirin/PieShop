using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShop.Core.SignalR
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            // Implement authorization logic  
            if (_httpContextAccessor.HttpContext.Request.Query.TryGetValue("username", out var username) &&
                username.Any() &&
                !string.IsNullOrWhiteSpace(username.FirstOrDefault()) &&
                username.FirstOrDefault() == "test_user")
            {
                // Authorization passed  
                context.Succeed(requirement);
            }
            else
            {
                // Authorization failed  
                context.Fail();
            }

            // Return completed task  
            return Task.CompletedTask;
        }
    }
}
