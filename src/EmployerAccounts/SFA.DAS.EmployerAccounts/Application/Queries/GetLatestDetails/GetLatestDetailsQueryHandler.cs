using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Strategies;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails
{
    public class GetLatestDetailsQueryHandler(
        ILogger<GetLatestDetailsQueryHandler> logger,
        IOrganisationApiStrategyFactory organisationApiStrategyFactory)
        : IRequestHandler<GetLatestDetailsQuery, GetLatestDetailsResult>
    {
        public async Task<GetLatestDetailsResult> Handle(GetLatestDetailsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting Latest Details for Organisation with identifier: {Identifier}", request.Identifier);

            var organisationApiStrategy = organisationApiStrategyFactory.CreateStrategy(request.OrganisationType);
            return await organisationApiStrategy.GetOrganisationDetails(request.Identifier, request.OrganisationType);
        }
    }
}
