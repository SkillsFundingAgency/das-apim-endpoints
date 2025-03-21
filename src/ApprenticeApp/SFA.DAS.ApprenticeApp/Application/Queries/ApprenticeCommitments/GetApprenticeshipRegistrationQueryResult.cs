using SFA.DAS.ApprenticeApp.Models;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipRegistration
{
    public class GetApprenticeshipRegistrationQueryResult
    {
        public Guid RegistrationId { get; set; }
        public long ApprenticeshipId { get; set; }
        public Guid? ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UserIdentityId { get; set; }
        public string CourseName { get; set; }
        public DateTime? StoppedReceivedOn { get; set; }
    }
}
