using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsQueryHandler : IRequestHandler<GetInterviewAdjustmentsQuery, GetInterviewAdjustmentsQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetInterviewAdjustmentsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetInterviewAdjustmentsQueryResult> Handle(GetInterviewAdjustmentsQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId));
        var interviewAdjustmentsTask = _candidateApiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(request.ApplicationId, request.CandidateId));

        await Task.WhenAll(applicationTask, interviewAdjustmentsTask);

        var application = applicationTask.Result;
        var interviewAdjustments = interviewAdjustmentsTask.Result;

        bool? isCompleted = application.InterviewAdjustmentsStatus switch
        {
            Constants.SectionStatus.InProgress => true,
            Constants.SectionStatus.Completed => false,
            _ => null
        };

        return new GetInterviewAdjustmentsQueryResult
        {
            ApplicationId = application.Id,
            InterviewAdjustmentsDescription = interviewAdjustments.AboutYou is null ? null : interviewAdjustments.AboutYou.Support,
            IsSectionCompleted = isCompleted
        };
    }
}
