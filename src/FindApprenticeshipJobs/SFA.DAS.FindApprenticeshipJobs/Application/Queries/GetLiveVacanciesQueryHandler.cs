using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
    private readonly ILiveVacancyMapper _liveVacancyMapper;

    public GetLiveVacanciesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ILiveVacancyMapper liveVacancyMapper)
    {
        _recruitApiClient = recruitApiClient;
        _liveVacancyMapper = liveVacancyMapper;
    }

    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response = await _recruitApiClient.GetWithResponseCode<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize));
        response.Body.Vacancies = RemoveTraineeships(response.Body);

        return new GetLiveVacanciesQueryResult
        {
            PageSize = response.Body.PageSize,
            PageNo = response.Body.PageNo,
            TotalLiveVacanciesReturned = response.Body.TotalLiveVacanciesReturned,
            TotalLiveVacancies = response.Body.TotalLiveVacancies,
            TotalPages = response.Body.TotalPages,
            Vacancies = await Task.WhenAll(response.Body.Vacancies.Select(x => _liveVacancyMapper.Map(x)))
                Route = standards.Standards.SingleOrDefault(s => s.LarsCode.ToString() == x.ProgrammeId) ?.Route ?? string.Empty,
                },
                OutcomeDescription = x.OutcomeDescription,
                //LongDescription =
                TrainingDescription = x.TrainingDescription,
                Skills = x.Skills,
                Qualifications = x.Qualifications.Select(q => new GetLiveVacanciesQueryResult.Qualification()
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = q.Weighting
                }).ToList(),
                ThingsToConsider = x.ThingsToConsider,
                Id = x.Id,
                IsDisabilityConfident = (GetLiveVacanciesQueryResult.DisabilityConfident)x.DisabilityConfident,
                IsEmployerAnonymous = x.IsAnonymous,
                EmployerDescription = x.EmployerDescription,
                EmployerWebsiteUrl = x.EmployerWebsiteUrl,
                IsRecruitVacancy = true,
                //AnonymousEmployerName = 
                //Category = 
                //CategoryCode = 
                //IsPositiveAboutDisability = 
                //SubCategory = 
                //SubCategoryCode = 
                //VacancyLocationType =
                //WageAmountLowerBand =
                //WageAmountUpperBand = 
                //ExpectedDuration = 
                //Distance = 
                //Score = 
                EmployerContactName = x.EmployerContact == null ? null : x.EmployerContact.EmployerContactName,
                EmployerContactEmail = x.EmployerContact == null ? null : x.EmployerContact.EmployerContactEmail,
                EmployerContactPhone = x.EmployerContact == null ? null : x.EmployerContact.EmployerContactPhone
        };
    }

    private static List<LiveVacancy> RemoveTraineeships(GetLiveVacanciesApiResponse response)
    {
        return response.Vacancies.Select(x => x)
            .Where(x => x.VacancyType == VacancyType.Apprenticeship)
            .ToList();
    }
}
