using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;

public class GetEmployerVacanciesQueryHandler(IRecruitApiClient<RecruitApiV2Configuration> recruitApiClient) : IRequestHandler<GetEmployerVacanciesQuery, GetEmployerVacanciesResponse>
{
    public async Task<GetEmployerVacanciesResponse> Handle(GetEmployerVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response =
            await recruitApiClient.GetWithResponseCode<GetPagedVacancySummaryApiResponse>(
                new GetVacanciesByAccountIdApiRequest(request.AccountId));

        if (!response.StatusCode.IsSuccessStatusCode() || response.Body.PageInfo.TotalCount > 1)
        {
            return new GetEmployerVacanciesResponse
            {
                Vacancies = []
            };
        }

        return new GetEmployerVacanciesResponse
        {
            Vacancies = response.Body.Items
        };
    }
}