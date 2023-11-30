using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
    private readonly ICourseService _courseService;

    public GetLiveVacanciesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ICourseService courseService)
    {
        _recruitApiClient = recruitApiClient;
        _courseService = courseService;
    }

    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response = await _recruitApiClient.GetWithResponseCode<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize));
        response.Body.Vacancies = RemoveTraineeships(response.Body);
        var standards = await _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        return new GetLiveVacanciesQueryResult
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
                NumberOfPositions = x.NumberOfPositions,
                ApprenticeshipTitle = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == x.ProgrammeId)?.Title ?? string.Empty,
                Level = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == x.ProgrammeId)?.Level ?? 0,
                Description = x.Description,
                EmployerName = x.EmployerName,
                ProviderName = x.TrainingProvider.Name,
                ProviderId = x.TrainingProvider.Ukprn,
                LiveDate = x.LiveDate,
                ClosingDate = x.ClosingDate,
                ProgrammeId = x.ProgrammeId,
                ProgrammeType = x.ProgrammeType,
                StartDate = x.StartDate,
                Route = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == x.ProgrammeId) ?.Route ?? string.Empty,
                EmployerLocation = new GetLiveVacanciesQueryResult.Address
                {
                    AddressLine1 = x.EmployerLocation?.AddressLine1,
                    AddressLine2 = x.EmployerLocation?.AddressLine2,
                    AddressLine3 = x.EmployerLocation?.AddressLine3,
                    AddressLine4 = x.EmployerLocation?.AddressLine4,
                    Postcode = x.EmployerLocation?.Postcode,
                    Latitude = x.EmployerLocation?.Latitude ?? 0,
                    Longitude = x.EmployerLocation?.Longitude ?? 0,
                },
                Wage = x.Wage == null ? null : new GetLiveVacanciesQueryResult.Wage
                {
                    Duration = x.Wage.Duration,
                    DurationUnit = x.Wage.DurationUnit,
                    FixedWageYearlyAmount = x.Wage.FixedWageYearlyAmount,
                    WageAdditionalInformation = x.Wage.WageAdditionalInformation,
                    WageType = x.Wage.WageType,
                    WeeklyHours = x.Wage.WeeklyHours,
                    WorkingWeekDescription = x.Wage.WorkingWeekDescription
                }
            }),
        };
    }

    private static List<LiveVacancy> RemoveTraineeships(GetLiveVacanciesApiResponse response)
    {
        return response.Vacancies.Select(x => x)
            .Where(x => x.VacancyType == VacancyType.Apprenticeship)
            .ToList();
    }
}
