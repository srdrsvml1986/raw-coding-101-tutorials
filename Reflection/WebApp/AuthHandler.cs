﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class AutoGeneratedClaim : IAuthorizationRequirement
    {
        public AutoGeneratedClaim()
        {}
    }

    public class AuthHandler : AuthorizationHandler<AutoGeneratedClaim>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AutoGeneratedClaim requirement)
        {
            var resource = context.Resource;
            if(resource is RouteEndpoint endpoint)
            {
                var metadata = endpoint.Metadata;
                var controllerAD = (ControllerActionDescriptor)
                    metadata.FirstOrDefault(x => x is ControllerActionDescriptor);

                var controller = controllerAD.ControllerTypeInfo;
                var action = controllerAD.MethodInfo;
                var targetClaim = string.Concat(controller.ToString(), ".", action.ToString().Split(" ").Last());

                if(context.User.Claims.Any(x => x.Type.Equals(Constants.WebAppClaimType)
                    && x.Value.Equals(targetClaim)))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
