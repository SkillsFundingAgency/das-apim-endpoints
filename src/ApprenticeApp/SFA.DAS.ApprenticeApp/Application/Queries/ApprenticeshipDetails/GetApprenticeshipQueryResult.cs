namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipDetails
{
    public class GetApprenticeshipQueryResult
    {
        public long Id { get; set; }
        public long EmployerAccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string EmployerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Uln { get; set; }
        public string StandardUId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Option { get; set; }
    }
}
