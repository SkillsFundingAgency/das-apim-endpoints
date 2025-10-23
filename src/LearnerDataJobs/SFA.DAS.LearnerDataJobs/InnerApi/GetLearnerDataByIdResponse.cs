using SFA.DAS.LearnerDataJobs.Application.Queries;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class GetLearnerDataByIdResponse
{
    public long? ApprenticeshipId { get; set; }

    public static GetLearnerByIdResult Mapfrom(GetLearnerDataByIdResponse response)
    {
        return new GetLearnerByIdResult() { ApprenticeshipId = response.ApprenticeshipId };
    }
}