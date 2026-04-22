namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments
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
