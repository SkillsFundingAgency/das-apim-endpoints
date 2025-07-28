using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;
using static System.Enum;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryHandler(
    IVacancyService vacancyService,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applicationsTask =
            candidateApiClient.Get<GetApplicationsApiResponse>(
                new GetApplicationsApiRequest(request.CandidateId));

        var totalApplicationCount = applicationsTask.Result.Applications.Count;
        var applicationList = applicationsTask.Result.Applications.Where(x =>
            x.Status == request.Status.ToString()
            || (request.Status == ApplicationStatus.Submitted && x.Status == ApplicationStatus.Withdrawn.ToString())
            || (request.Status == ApplicationStatus.Draft && x.Status == ApplicationStatus.Expired.ToString())
            ).ToList();

        if (totalApplicationCount == 0 || applicationList.Count == 0)
        {
            return new GetApplicationsQueryResult();
        }

        var vacancyReferences = applicationList.Select(x => $"{x.VacancyReference.TrimVacancyReference()}").ToList();
        
        var vacancies = await vacancyService.GetVacancies(vacancyReferences);

        var result = new GetApplicationsQueryResult();

        foreach (var application in applicationList)
        {
            var vacancy = vacancies.FirstOrDefault(v => v.VacancyReference.TrimVacancyReference() == application.VacancyReference);

            if (vacancy is null) continue;

            TryParse<ApplicationStatus>(application.Status, out var status);
            result.Applications.Add(new GetApplicationsQueryResult.Application
            {
                Id = application.Id,
                VacancyReference = vacancy.VacancyReference,
                EmployerName = vacancy.EmployerName,
                Title = vacancy.Title,
                ClosingDate = vacancy.ClosingDate,
                ClosedDate = vacancy.ClosedDate,
                CreatedDate = application.CreatedDate,
                WithdrawnDate = application.WithdrawnDate,
                Status = status,
                SubmittedDate = application.SubmittedDate,
                ResponseDate = application.ResponseDate,
                ResponseNotes = application.ResponseNotes,
                Address = vacancy.Address,
                EmploymentLocationInformation = vacancy.EmploymentLocationInformation,
                EmployerLocationOption = vacancy.EmployerLocationOption,
                OtherAddresses = vacancy.OtherAddresses?.ToList(),
                ApprenticeshipType = vacancy.ApprenticeshipType,
            });
        }

        return result;
    }
}