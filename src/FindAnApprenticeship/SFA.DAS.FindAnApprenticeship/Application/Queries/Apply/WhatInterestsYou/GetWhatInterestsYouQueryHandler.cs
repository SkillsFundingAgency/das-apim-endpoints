using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WhatInterestsYou;

public class GetWhatInterestsYouQueryHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetWhatInterestsYouQuery, GetWhatInterestsYouQueryResult>
{
    public async Task<GetWhatInterestsYouQueryResult> Handle(GetWhatInterestsYouQuery request, CancellationToken cancellationToken)
    {
        var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

        var vacancyRequest = new GetVacancyRequest(application.VacancyReference);
        var vacancy = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(vacancyRequest);

        return new GetWhatInterestsYouQueryResult
        {
            EmployerName = vacancy.EmployerName,
            StandardName = vacancy.CourseTitle
        };
    }
}