using System.Net;
using System.ComponentModel;

namespace SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;

public class GetApprenticeshipsResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public List<ApprenticeshipDetailsResponse>? Apprenticeships { get; set; }
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
        public DateTime StopDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
    }
}

public enum DeliveryModel : byte
{
    Regular = 0,
    PortableFlexiJob = 1,
    FlexiJobAgency = 2
}

public enum ApprenticeshipStatus : short
{
    [Description("Waiting to start")]
    WaitingToStart = 0,
    [Description("Live")]
    Live = 1,
    [Description("Paused")]
    Paused = 2,
    [Description("Stopped")]
    Stopped = 3,
    [Description("Completed")]
    Completed = 4,
    [Description("Unknown")]
    Unknown
}