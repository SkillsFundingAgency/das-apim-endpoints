using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;

public class GetApplicationsQueryHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
    : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications =
            await candidateApiClient.Get<GetApplicationsApiResponse>(
                new GetApplicationsApiRequest(request.CandidateId, request.Status));

        if (applications.Applications.Count == 0)  { return new GetApplicationsQueryResult(); }

        var vacancyReferences = applications.Applications.Select(x => $"VAC{x.VacancyReference}");

        var vacanciesRequest = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
        {
            VacancyReferences = vacancyReferences.ToList()
        });

        var vacancies = await findApprenticeshipApiClient.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(vacanciesRequest);

        var result = new GetApplicationsQueryResult();

        foreach (var application in applications.Applications)
        {
            var vacancy = vacancies.Body.ApprenticeshipVacancies.Single(v => v.VacancyReference == $"VAC{application.VacancyReference}");

            result.Applications.Add(new GetApplicationsQueryResult.Application
            {
                Id = application.Id,
                VacancyReference = vacancy.VacancyReference,
                EmployerName = vacancy.EmployerName,
                Title = vacancy.Title,
                ClosingDate = vacancy.ClosingDate,
                CreatedDate = application.CreatedDate,
                Status = request.Status,
                SubmittedDate = application.SubmittedDate
            });
        }

        return result;
    }
}