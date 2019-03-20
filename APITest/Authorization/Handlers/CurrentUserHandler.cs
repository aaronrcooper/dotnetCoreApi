using System.Linq;
using System.Threading.Tasks;
using APITest.Authorization.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace APITest.Authorization.Handlers
{
    public class CurrentUserHandler : AuthorizationHandler<CurrentUserRequirement, string>
    {
        private IConfiguration _config { get; set; }
        public CurrentUserHandler(IConfiguration config)
        {
            _config = config;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CurrentUserRequirement requirement, string userId)
        {
            // Get the ID from the claim
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId && c.Issuer == _config.GetSection("jwt").GetSection("issuer").Value);

            if (claim.Value == userId)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
