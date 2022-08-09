using System.Net;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetApprenticeshipsResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public IEnumerable<ApprenticeshipDetailsResponse>? Apprenticeships { get; set; }
    public int TotalApprenticeshipsFound { get; set; }

    public class ApprenticeshipDetailsResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Uln { get; set; } = string.Empty;
        public string EmployerName { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public long ProviderId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PauseDate { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}