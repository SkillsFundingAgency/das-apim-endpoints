using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Authorization;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Api.Authentication
{
    public class PledgeAuthorizationHandler : AuthorizationHandler<PledgeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PledgeAuthorizationHandler> _logger;
        private readonly ILevyTransferMatchingService _service;

        public PledgeAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
            ILogger<PledgeAuthorizationHandler> logger,
            ILevyTransferMatchingService service)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _service = service;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PledgeRequirement requirement)
        {
            var isAuthorized = await IsEmployerAuthorized(context);

            if (isAuthorized)
            {
                _logger.LogInformation($"PledgeRequirement met");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation($"PledgeRequirement not met");
            }
        }

        private async Task<bool> IsEmployerAuthorized(AuthorizationHandlerContext context)
        {
            if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountId) || !_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.PledgeId))
            {
                return false;
            }

            var accountIdFromUrl = long.Parse(_httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountId].ToString());
            var pledgeIdFromUrl = int.Parse(_httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.PledgeId].ToString());
            var pledge = await _service.GetPledge(pledgeIdFromUrl);

            return pledge.AccountId == accountIdFromUrl;
        }
    }
}