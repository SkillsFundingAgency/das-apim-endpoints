using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.Cmad.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRevisionById
{
    public class GetRevisionsByIdQueryHandler : IRequestHandler<GetRevisionsByIdQuery, GetRevisionsByIdQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;

        public GetRevisionsByIdQueryHandler(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> commitmentsApiClient)
            => _commitmentsApiClient = commitmentsApiClient;

        public async Task<GetRevisionsByIdQueryResult> Handle(GetRevisionsByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _commitmentsApiClient.Get<Revision>(
                new GetRevisionsByIdRequest(request.ApprenticeId, request.ApprenticeshipId, request.RevisionId));            

            return new GetRevisionsByIdQueryResult { Revision = result};
        }
    }
}
