using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses
{
    public class GetCalculatedVersionOfTrainingProgrammeRequest : IGetApiRequest
    {
        public readonly string CourseCode;
        public readonly DateTime StartDate;

        public GetCalculatedVersionOfTrainingProgrammeRequest(string courseCode, DateTime? startDate)
        {
            CourseCode = courseCode;
            StartDate = startDate.HasValue ? startDate.Value : DateTime.Today;
        }

        public string GetUrl => $"api/TrainingProgramme/calculate-version/{CourseCode}?startDate={StartDate.ToString("yyyy-MM-dd")}";
    }
}