using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryHandler(
    IVacancyService vacancyService,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var candidateApiResponseTask =
            candidateApiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId.ToString()));

        var applicationsTask =
            candidateApiClient.Get<GetApplicationsApiResponse>(
                new GetApplicationsApiRequest(request.CandidateId, request.Status));

        var candidateApiResponse = candidateApiResponseTask.Result;
        var applicationList = applicationsTask.Result.Applications;

        if (request.Status == ApplicationStatus.Submitted)
        {
            var withdrawnApplications =
                await candidateApiClient.Get<GetApplicationsApiResponse>(
                    new GetApplicationsApiRequest(request.CandidateId, ApplicationStatus.Withdrawn));
            applicationList.AddRange(withdrawnApplications.Applications);
        }
        
        if (applicationList.Count == 0)  { return new GetApplicationsQueryResult(); }

        var vacancyReferences = applicationList.Select(x => $"{x.VacancyReference}").ToList();
        
        var vacancies = await vacancyService.GetVacancies(vacancyReferences);

        var result = new GetApplicationsQueryResult
        {
            ShowAccountRecoveryBanner = string.IsNullOrWhiteSpace(candidateApiResponse.MigratedEmail) && applicationList.Count == 0
        };

        foreach (var application in applicationList)
        {
            var vacancy = vacancies.FirstOrDefault(v => v.VacancyReference.Replace("VAC", string.Empty) == application.VacancyReference);
            Enum.TryParse<ApplicationStatus>(application.Status, out var status);
            result.Applications.Add(new GetApplicationsQueryResult.Application
            {
                Id = application.Id,
                VacancyReference = vacancy?.VacancyReference,
                EmployerName = vacancy?.EmployerName,
                Title = vacancy?.Title,
                ClosingDate = vacancy?.ClosedDate ?? vacancy!.ClosingDate,
                CreatedDate = application.CreatedDate,
                WithdrawnDate = application.WithdrawnDate,
                Status = status,
                SubmittedDate = application.SubmittedDate,
                ResponseDate = application.ResponseDate,
                ResponseNotes = application.ResponseNotes
            });
        }

        return result;
    }
}