using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Vacancies;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Vacancies;
using SFA.DAS.Recruit.InnerApi.Mappers;
using System.Net;

namespace SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyByReference;

public class GetVacancyByReferenceQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
    : IRequestHandler<GetVacancyByReferenceQuery, GetVacancyByReferenceQueryResult>
{
    public async Task<GetVacancyByReferenceQueryResult> Handle(GetVacancyByReferenceQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<GetVacancyByReferenceResponse>(
            new GetVacancyByReferenceRequest(request.VacancyReference));
        return response.StatusCode == HttpStatusCode.NotFound
            ? GetVacancyByReferenceQueryResult.None
            : new GetVacancyByReferenceQueryResult(response.Body.ToDomain());
    }
}
