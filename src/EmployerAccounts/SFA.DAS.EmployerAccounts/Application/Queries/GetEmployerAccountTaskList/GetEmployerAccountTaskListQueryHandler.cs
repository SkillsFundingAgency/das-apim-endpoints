using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList
{
    public class GetEmployerAccountTaskListQueryHandler : IRequestHandler<GetEmployerAccountTaskListQuery, GetEmployerAccountTaskListQueryResult>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;

        public GetEmployerAccountTaskListQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task<GetEmployerAccountTaskListQueryResult> Handle(GetEmployerAccountTaskListQuery request, CancellationToken cancellationToken)
        {
            var providerRelationshipResponse =
                await _providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetEmployerAccountProviderPermissionsRequest(request.HashedAccountId));

            var employerAlePermissions = providerRelationshipResponse.AccountProviderLegalEntities.Select(aple => new AccountLegalEntityItem
            {
                Name = aple.AccountLegalEntityName,
                AccountLegalEntityPublicHashedId = aple.AccountLegalEntityPublicHashedId,
                AccountHashedId = aple.AccountHashedId
            });

            return new GetEmployerAccountTaskListQueryResult
            {
                EmployerAccountLegalEntityPermissions = employerAlePermissions
            };
        }
    }
}