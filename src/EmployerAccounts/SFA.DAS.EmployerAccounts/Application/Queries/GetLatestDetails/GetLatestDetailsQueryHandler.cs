using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Strategies;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails
{
    public class GetLatestDetailsQueryHandler : IRequestHandler<GetLatestDetailsQuery, GetLatestDetailsResult>
    {
        private readonly ILogger<GetLatestDetailsQueryHandler> _logger;
        private readonly IOrganisationApiStrategyFactory _organisationApiStrategyFactory;

        public GetLatestDetailsQueryHandler(ILogger<GetLatestDetailsQueryHandler> logger,
                   IOrganisationApiStrategyFactory organisationApiStrategyFactory)
        {
            _logger = logger;
            _organisationApiStrategyFactory = organisationApiStrategyFactory;
        }

        public async Task<GetLatestDetailsResult> Handle(GetLatestDetailsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting Latest Details for Organisation with identifier: {Identifier}", request.Identifier);

            var organisationApiStrategy = _organisationApiStrategyFactory.CreateStrategy(request.OrganisationType);
            return await organisationApiStrategy.GetOrganisationDetails(request.Identifier, request.OrganisationType);
        }
    }
}
