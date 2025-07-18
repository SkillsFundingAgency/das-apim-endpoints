using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQueryHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService)
    : IRequestHandler<GetApplicationSubmittedQuery, GetApplicationSubmittedQueryResult>
{
    public async Task<GetApplicationSubmittedQueryResult> Handle(GetApplicationSubmittedQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        var aboutYouTask = candidateApiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(request.CandidateId));

        await Task.WhenAll(applicationTask, aboutYouTask);

        var application = applicationTask.Result;
        var aboutYou = aboutYouTask.Result;

        if (application is null) return null;

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference)
                      ?? await vacancyService.GetClosedVacancy(application.VacancyReference);

        if (vacancy is null) return null;

        return new GetApplicationSubmittedQueryResult
        {
            VacancyTitle = vacancy.Title,
            EmployerName = vacancy.EmployerName,
            HasAnsweredEqualityQuestions = aboutYou?.AboutYou != null,
            ClosedDate = vacancy.ClosedDate,
            ClosingDate = vacancy.ClosingDate,
        };
    }
}
