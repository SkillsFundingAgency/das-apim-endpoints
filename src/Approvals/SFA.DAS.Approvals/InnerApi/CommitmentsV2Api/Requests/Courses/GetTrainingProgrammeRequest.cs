using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses
{
    public class GetTrainingProgrammeRequest : IGetApiRequest
    {
        public readonly string CourseCode;

        public GetTrainingProgrammeRequest(string courseCode)
        {
            CourseCode = courseCode;
        }

        public string GetUrl => $"api/TrainingProgramme/{CourseCode}";
    }
}