using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQueryHandler : IRequestHandler<GetApplicationSubmittedQuery, GetApplicationSubmittedQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetApplicationSubmittedQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetApplicationSubmittedQueryResult> Handle(GetApplicationSubmittedQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        var aboutYouTask = _candidateApiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(request.CandidateId));

        await Task.WhenAll(applicationTask, aboutYouTask);

        var application = applicationTask.Result;
        var aboutYou = aboutYouTask.Result;

        if (application is null) return null;

        var vacancy = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(application.VacancyReference.ToString()));
        if (vacancy is null) return null;

        return new GetApplicationSubmittedQueryResult
        {
            VacancyTitle = vacancy.Title,
            EmployerName = vacancy.EmployerName,
            HasAnsweredEqualityQuestions = aboutYou?.AboutYou != null,
        };
    }
}
