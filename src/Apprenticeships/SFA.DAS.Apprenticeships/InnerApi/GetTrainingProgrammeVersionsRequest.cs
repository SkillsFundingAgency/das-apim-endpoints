using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.InnerApi;

public class GetTrainingProgrammeVersionsRequest : IGetApiRequest
{
    public readonly string CourseCode;
    public string GetUrl => $"api/trainingprogramme/{CourseCode}/versions";

    public GetTrainingProgrammeVersionsRequest(string courseCode)
    {
        CourseCode = courseCode;
    }
}