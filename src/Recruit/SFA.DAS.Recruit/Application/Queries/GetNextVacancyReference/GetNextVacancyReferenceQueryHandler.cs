using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetNextVacancyReference;

public class GetNextVacancyReferenceQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetNextVacancyReferenceQuery, GetNextVacancyReferenceQueryResult>
{
    public async Task<GetNextVacancyReferenceQueryResult> Handle(GetNextVacancyReferenceQuery request, CancellationToken cancellationToken)
    {
        var result = await recruitApiClient.GetWithResponseCode<GetNextVacancyReferenceResponse>(new GetNextVacancyReferenceRequest());
        return new GetNextVacancyReferenceQueryResult
        {
            Value = result.Body.NextVacancyReference
        };
    }
}