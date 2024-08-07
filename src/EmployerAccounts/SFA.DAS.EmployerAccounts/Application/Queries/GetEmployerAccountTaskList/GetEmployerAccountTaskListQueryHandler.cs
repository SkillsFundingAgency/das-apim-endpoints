﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

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
            var accountProvidersResponse =
                await _providerRelationshipsApiClient.Get<GetAccountProvidersResponse>(
                    new GetAccountProvidersRequest(request.AccountId));

            if (!accountProvidersResponse.AccountProviders.Any())
            {
                return new GetEmployerAccountTaskListQueryResult();
            }

            var providerRelationshipResponse =
                await _providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.HashedAccountId, new List<Operation> { Operation.Recruitment, Operation.RecruitmentRequiresReview }));

            var employerAlePermissions = providerRelationshipResponse.AccountProviderLegalEntities.Select(aple => new AccountLegalEntityItem
            {
                Name = aple.AccountLegalEntityName,
                AccountLegalEntityPublicHashedId = aple.AccountLegalEntityPublicHashedId,
                AccountHashedId = aple.AccountHashedId
            });

            return new GetEmployerAccountTaskListQueryResult
            {
                HasProviders = true,
                HasPermissions = employerAlePermissions.Any()
            };
        }
    }
}