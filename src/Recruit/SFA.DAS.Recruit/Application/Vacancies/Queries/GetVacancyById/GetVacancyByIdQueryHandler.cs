using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Mappers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Vacancies;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Vacancies;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;

public class GetVacancyByIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetVacancyByIdQuery, GetVacancyByIdQueryResult>
{
    public async Task<GetVacancyByIdQueryResult> Handle(GetVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<GetVacancyByIdResponse>(new GetVacancyByIdRequest(request.Id));
        return response.StatusCode == HttpStatusCode.NotFound
            ? GetVacancyByIdQueryResult.None
            : new GetVacancyByIdQueryResult(response.Body.ToDomain());
    }
}