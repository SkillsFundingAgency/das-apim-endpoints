using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAccountProvidersCourseStatusResponse
    {
        public List<AccountProviderCourse> Active { get; set; } = new();


        public List<AccountProviderCourse> Completed { get; set; } = new();


        public List<AccountProviderCourse> NewStart { get; set; } = new();
    }

    public sealed class AccountProviderCourse
    {
        public long Ukprn { get; set; }

        public string CourseCode { get; set; }
    }
}
