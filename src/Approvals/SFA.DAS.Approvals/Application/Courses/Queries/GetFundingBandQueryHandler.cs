using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Courses.Queries;

public class GetFundingBandQueryHandler : IRequestHandler<GetFundingBandQuery, GetFundingBandResult>
{
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

    public GetFundingBandQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetFundingBandResult> Handle(GetFundingBandQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.Get<GetTrainingProgrammeResponse>(new GetCalculatedVersionOfTrainingProgrammeRequest(request.CourseCode, request.StartDate));

        if (result?.TrainingProgramme == null)
            return null;

        return new GetFundingBandResult
        {
            StandardUId = result.TrainingProgramme.StandardUId,
            Version = result.TrainingProgramme.Version,
            StandardPageUrl = result.TrainingProgramme.StandardPageUrl,
            ProposedMaxFunding = result.TrainingProgramme.FundingPeriods.GetFundingBandForDate(request.StartDate),
        };
    }
}