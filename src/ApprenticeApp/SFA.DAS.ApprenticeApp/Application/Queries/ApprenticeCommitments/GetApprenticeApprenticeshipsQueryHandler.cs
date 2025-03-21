using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeCommitments
{
    public class GetApprenticeApprenticeshipsQueryHandler : IRequestHandler<GetApprenticeApprenticeshipsQuery, GetApprenticeApprenticeshipsQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apprenticeCommitmentsApiClient;

        public GetApprenticeApprenticeshipsQueryHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient)
        {
            _apprenticeCommitmentsApiClient = apprenticeCommitmentsApiClient;
        }

        public async Task<GetApprenticeApprenticeshipsQueryResult> Handle(GetApprenticeApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeCommitmentsApiClient.Get<GetApprenticeApprenticeshipsQueryResult>(new GetApprenticeApprenticeshipsRequest(request.ApprenticeId));
            return new GetApprenticeApprenticeshipsQueryResult
            {
                Apprenticeships = result.Apprenticeships
            };
        }
    }
}
