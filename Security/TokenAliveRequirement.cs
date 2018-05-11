using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SampleWebApi.Security
{
    public class TokenAliveRequirement : IAuthorizationRequirement
    {
        //public bool TokenAlive { get; set; }

        //public TokenAliveRequirement(bool isAlive)
        //{
        //    this.TokenAlive = isAlive;
        //}
    }

    public class TokenAliveHandler : AuthorizationHandler<TokenAliveRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TokenAliveRequirement requirement)
        {
            // could use this to find sessionId (token series) claim value
            var series = context.User.FindFirst(claim => claim.Type == "series").Value;

            // lookup token and check to make sure it is still alive
            //if (tokenAlive)
            //{
            //    context.Succeed(requirement);
            //}

            if (string.IsNullOrEmpty(series))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
