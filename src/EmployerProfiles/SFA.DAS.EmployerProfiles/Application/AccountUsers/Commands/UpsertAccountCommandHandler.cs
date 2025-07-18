using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands
{
    public class UpsertAccountCommandHandler : IRequestHandler<UpsertAccountCommand, EmployerProfile>
    {
        private readonly IEmployerAccountsService _employerAccountService;
        private readonly ILogger<UpsertAccountCommandHandler> _logger;

        public UpsertAccountCommandHandler(IEmployerAccountsService employerAccountService, ILogger<UpsertAccountCommandHandler> logger)
        {
            _employerAccountService = employerAccountService;
            _logger = logger;
        }
        public async Task<EmployerProfile> Handle(UpsertAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Upserting employer account for user {UserId} with correlation ID {CorrelationId}", 
                request.UserId, request.CorrelationId);

            var result = await _employerAccountService.PutEmployerAccount(new EmployerProfile
            {
                GovIdentifier = request.GovIdentifier,
                Email = request.Email,
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CorrelationId = request.CorrelationId
            });

            _logger.LogInformation("Successfully upserted employer account for user {UserId}", request.UserId);
            return result;
        }
    }
}
