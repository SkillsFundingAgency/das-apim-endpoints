using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin
{
    public class ApprenticeLoginServiceLogging : IApprenticeLoginService
    {
        private readonly IApprenticeLoginService _service;
        private readonly ILogger<ApprenticeLoginService> _logger;

        public ApprenticeLoginServiceLogging(IApprenticeLoginService service, ILogger<ApprenticeLoginService> logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task<bool> IsHealthy()
        {
            return _service.IsHealthy();
        }

        public async Task SendInvitation(Guid guid, string email)
        {
            try
            {
                _logger.LogInformation("Sending Invitation");
                await _service.SendInvitation(guid, email);
                _logger.LogInformation("Invitation Sent");

            }
            catch (Exception e)
            {
                _logger.LogError("Error Sending Invitation", e);
                throw;
            }
        }
    }
}