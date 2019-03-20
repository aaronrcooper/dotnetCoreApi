using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace APITest.Authorization.Rules
{
    public class CurrentUserRequirement : IAuthorizationRequirement
    {
        // this class is empty, we simply need a class of type IAuthorizationRequirement to pass to the authorization policy
        public CurrentUserRequirement() { }
    }
}
