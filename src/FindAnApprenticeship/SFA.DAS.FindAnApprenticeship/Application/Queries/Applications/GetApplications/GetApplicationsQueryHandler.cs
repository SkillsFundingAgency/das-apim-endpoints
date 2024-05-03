using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryHandler(
    IVacancyService vacancyService,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications =
            await candidateApiClient.Get<GetApplicationsApiResponse>(
                new GetApplicationsApiRequest(request.CandidateId, request.Status));

        if (applications.Applications.Count == 0)  { return new GetApplicationsQueryResult(); }

        var vacancyReferences = applications.Applications.Select(x => $"{x.VacancyReference}").ToList();
        
        var vacancies = await vacancyService.GetVacancies(vacancyReferences);

        var result = new GetApplicationsQueryResult();

        foreach (var application in applications.Applications)
        {
            var vacancy = vacancies.FirstOrDefault(v => v.VacancyReference.Replace("VAC", string.Empty) == application.VacancyReference);

            result.Applications.Add(new GetApplicationsQueryResult.Application
            {
                Id = application.Id,
                VacancyReference = vacancy?.VacancyReference,
                EmployerName = vacancy?.EmployerName,
                Title = vacancy?.Title,
                ClosingDate = vacancy!.ClosingDate,
                CreatedDate = application.CreatedDate,
                Status = request.Status,
                SubmittedDate = application.SubmittedDate,
                ResponseDate = application.ResponseDate,
                ResponseNotes = application.ResponseNotes
            });
        }

        return result;
    }
}