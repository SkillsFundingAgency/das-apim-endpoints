﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.Inform
{
    public class GetInformQueryHandler : IRequestHandler<GetInformQuery, GetInformQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetInformQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetInformQueryResult> Handle(GetInformQuery request, CancellationToken cancellationToken)
        {
            var apprenticeship = await _commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

            if (apprenticeship == null || apprenticeship.ProviderId != request.ProviderId)
            {
                return null;
            }

            return new GetInformQueryResult
            {
                ApprenticeshipId = request.ApprenticeshipId,
                LegalEntityName = apprenticeship.EmployerName,
            };
        }
    }
}
