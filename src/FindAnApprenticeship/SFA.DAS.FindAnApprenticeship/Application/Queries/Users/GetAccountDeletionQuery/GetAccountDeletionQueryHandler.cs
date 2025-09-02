using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using static System.Enum;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetAccountDeletionQuery
{
    public record GetAccountDeletionQueryHandler(
        IVacancyService VacancyService,
        ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient)
        : IRequestHandler<GetAccountDeletionQuery, GetAccountDeletionQueryResult>
    {
        public async Task<GetAccountDeletionQueryResult> Handle(GetAccountDeletionQuery request,
            CancellationToken cancellationToken)
        {
            var applicationsTask =
                CandidateApiClient.Get<GetApplicationsApiResponse>(new GetApplicationsApiRequest(request.CandidateId));

            var applicationList = applicationsTask.Result.Applications.Where(x =>
                    x.Status == ApplicationStatus.Submitted.ToString())
                .ToList();

            var vacancyReferences = applicationList.Select(x => $"{x.VacancyReference.TrimVacancyReference()}").ToList();

            var vacancies = await VacancyService.GetVacancies(vacancyReferences);

            var result = new GetAccountDeletionQueryResult();

            foreach (var application in applicationList)
            {
                var vacancy = vacancies.FirstOrDefault(v =>
                    v.VacancyReference.TrimVacancyReference() == application.VacancyReference);
                
                TryParse<ApplicationStatus>(application.Status, out var status);

                result.SubmittedApplications.Add(new GetAccountDeletionQueryResult.Application
                {
                    Address = vacancy?.Address,
                    ApprenticeshipType = vacancy.ApprenticeshipType,
                    ClosingDate = vacancy?.ClosedDate ?? vacancy!.ClosingDate,
                    CreatedDate = application.CreatedDate,
                    EmployerLocationOption = vacancy?.EmployerLocationOption,
                    EmployerName = vacancy?.EmployerName,
                    EmploymentLocationInformation = vacancy?.EmploymentLocationInformation,
                    Id = application.Id,
                    OtherAddresses = vacancy?.OtherAddresses,
                    Status = status,
                    SubmittedDate = application.SubmittedDate,
                    Title = vacancy?.Title,
                    VacancyReference = vacancy?.VacancyReference,
                });
            }

            return result;
        }
    }
}
