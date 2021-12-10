using MediatR;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeCommitments.Queries
{
    public class GetApprenticeApprenticeshipsQueryHandler : IRequestHandler<GetApprenticeApprenticeshipsQuery, GetApprenticeApprenticeshipsQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apiClient;

        public GetApprenticeApprenticeshipsQueryHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apiClient)
            => _apiClient = apiClient;

        public async Task<GetApprenticeApprenticeshipsQueryResult> Handle(GetApprenticeApprenticeshipsQuery request, CancellationToken cancellationToken)
            => await _apiClient.Get<GetApprenticeApprenticeshipsQueryResult>(new GetApprenticeApprenticeshipsRequest(request.ApprenticeId));
    }
}
