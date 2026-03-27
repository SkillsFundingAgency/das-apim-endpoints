using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Services;

public class TrainingProgrammeResolutionService(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
    : ITrainingProgrammeResolutionService
{
    public async Task<GetTrainingProgrammeResponse> GetTrainingProgrammeAsync(string courseCode, DateTime? startDate)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
        {
            return null;
        }

        if (int.TryParse(courseCode, out var standardId) && standardId > 0 && startDate.HasValue)
        {
            return await commitmentsApiClient.Get<GetTrainingProgrammeResponse>(
                new GetCalculatedTrainingProgrammeVersionRequest(standardId, startDate.Value));
        }

        return await commitmentsApiClient.Get<GetTrainingProgrammeResponse>(
            new GetTrainingProgrammeRequest(courseCode));
    }
}
