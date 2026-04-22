using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;

public class GetAllTrainingProgrammesRequest: IGetApiRequest
{
    public string GetUrl => $"api/TrainingProgramme/all";
}
