using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SubmitEmployerRequest
{
    public class SubmitEmployerRequestCommand : IRequest<SubmitEmployerRequestResponse>
    {
        public string OriginalLocation { get; set; }
        public RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public double SingleLocationLatitude { get; set; }
        public double SingleLocationLongitude { get; set; }
        public string[] MultipleLocations { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public string RequestedByEmail { get; set; }
        public Guid ModifiedBy { get; set; }
        public string CourseLevel { get; set; }
        public string RequestedByFirstName { get; set; }
        public int ExpiryAfterMonths { get; set; }
        public string DashboardUrl { get; set; }
    }
}
