using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;

    public GetLiveVacanciesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    {
        _recruitApiClient = recruitApiClient;
    }

    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response = await _recruitApiClient.GetWithResponseCode<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize));

        return new GetLiveVacanciesQueryResult()
        {
            PageSize = response.Body.PageSize,
            PageNo = response.Body.PageNo,
            TotalLiveVacanciesReturned = response.Body.TotalLiveVacanciesReturned,
            TotalLiveVacancies = response.Body.TotalLiveVacancies,
            TotalPages = response.Body.TotalPages,
            Vacancies = response.Body.Vacancies.Select(x => new GetLiveVacanciesQueryResult.LiveVacancy
            {
                VacancyId = x.VacancyId,
                VacancyTitle = x.Title,
                ApprenticeshipTitle = x.Title, //todo: source from courses service
                Description = x.Description,
                EmployerName = x.EmployerName,
                LiveDate = x.LiveDate,
                ProgrammeId = x.ProgrammeId,
                ProgrammeType = x.ProgrammeType,
                StartDate = x.StartDate,
                RouteId = x.RouteId, //todo: get from courses service probably
                EmployerLocation = new GetLiveVacanciesQueryResult.Address
                {
                    AddressLine1 = x.EmployerLocation?.AddressLine1,
                    AddressLine2 = x.EmployerLocation?.AddressLine2,
                    AddressLine3 = x.EmployerLocation?.AddressLine3,
                    AddressLine4 = x.EmployerLocation?.AddressLine4,
                    Postcode = x.EmployerLocation?.Postcode,
                    Latitude = x.EmployerLocation?.Latitude ?? 0,
                    Longitude = x.EmployerLocation?.Longitude ?? 0,
                }
            }),
        };
    }
}
