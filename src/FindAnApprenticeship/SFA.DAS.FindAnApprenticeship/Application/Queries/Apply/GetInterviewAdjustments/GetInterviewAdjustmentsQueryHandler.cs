using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetInterviewAdjustmentsQuery, GetInterviewAdjustmentsQueryResult>
{
    public async Task<GetInterviewAdjustmentsQueryResult> Handle(GetInterviewAdjustmentsQuery request, CancellationToken cancellationToken)
    {
        var applicationTask = candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        var interviewAdjustmentsTask = candidateApiClient.Get<GetAboutYouItemApiResponse>(new GetAboutYouItemApiRequest(request.ApplicationId, request.CandidateId));

        await Task.WhenAll(applicationTask, interviewAdjustmentsTask);

        var application = applicationTask.Result;
        var interviewAdjustments = interviewAdjustmentsTask.Result;

        bool? isCompleted = application.InterviewAdjustmentsStatus switch
        {
            Constants.SectionStatus.Incomplete => false,
            Constants.SectionStatus.Completed => true,
            _ => null
        };

        return new GetInterviewAdjustmentsQueryResult
        {
            ApplicationId = application.Id,
            InterviewAdjustmentsDescription = interviewAdjustments.AboutYou?.Support,
            IsSectionCompleted = isCompleted
        };
    }
}
