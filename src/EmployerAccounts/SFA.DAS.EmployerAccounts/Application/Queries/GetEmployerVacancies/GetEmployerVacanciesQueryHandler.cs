using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerVacancies;

public class GetEmployerVacanciesQueryHandler(IRecruitApiClient<RecruitApiV2Configuration> recruitApiClient) : IRequestHandler<GetEmployerVacanciesQuery, GetEmployerVacanciesQueryResponse>
{
    public async Task<GetEmployerVacanciesQueryResponse> Handle(GetEmployerVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response =
            await recruitApiClient.GetWithResponseCode<GetPagedVacancySummaryApiResponse>(
                new GetVacanciesByAccountIdApiRequest(request.AccountId));

        if (!response.StatusCode.IsSuccessStatusCode() || response.Body.PageInfo.TotalCount > 1)
        {
            return new GetEmployerVacanciesQueryResponse
            {
                Vacancies = []
            };
        }

        return new GetEmployerVacanciesQueryResponse
        {
            Vacancies = response.Body.Items
        };
    }
}