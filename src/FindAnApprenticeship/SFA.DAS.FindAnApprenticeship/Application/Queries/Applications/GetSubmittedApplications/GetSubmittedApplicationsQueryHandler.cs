using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsQueryHandler(
        IVacancyService VacancyService,
        ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient)
        : IRequestHandler<GetSubmittedApplicationsQuery, GetSubmittedApplicationsQueryResult>
    {
        public async Task<GetSubmittedApplicationsQueryResult> Handle(GetSubmittedApplicationsQuery request,
            CancellationToken cancellationToken)
        {
            var applicationsTask =
                CandidateApiClient.Get<GetApplicationsApiResponse>(new GetApplicationsApiRequest(request.CandidateId));

            var applicationList = applicationsTask.Result.Applications.Where(x =>
                    x.Status == ApplicationStatus.Submitted.ToString())
                .ToList();

            var vacancyReferences = applicationList.Select(x => $"{x.VacancyReference}").ToList();

            var vacancies = await VacancyService.GetVacancies(vacancyReferences);

            var result = new GetSubmittedApplicationsQueryResult();

            foreach (var application in applicationList)
            {
                var vacancy = vacancies.FirstOrDefault(v =>
                    v.VacancyReference.Replace("VAC", string.Empty) == application.VacancyReference);
                Enum.TryParse<ApplicationStatus>(application.Status, out var status);
                result.SubmittedApplications.Add(new GetSubmittedApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    City = vacancy?.City,
                    Postcode = vacancy?.Postcode,
                    VacancyReference = vacancy?.VacancyReference,
                    EmployerName = vacancy?.EmployerName,
                    Title = vacancy?.Title,
                    ClosingDate = vacancy?.ClosedDate ?? vacancy!.ClosingDate,
                    CreatedDate = application.CreatedDate,
                    Status = status,
                    SubmittedDate = application.SubmittedDate,
                });
            }

            return result;
        }
    }
}
