using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetVacancyDetails
{
    public class GetVacancyDetailsQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
        : IRequestHandler<GetVacancyDetailsQuery, GetVacancyDetailsQueryResult>
    {
        public async Task<GetVacancyDetailsQueryResult> Handle(GetVacancyDetailsQuery request, CancellationToken cancellationToken)
        {
            return await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.VacancyReference));
        }
    }
}
