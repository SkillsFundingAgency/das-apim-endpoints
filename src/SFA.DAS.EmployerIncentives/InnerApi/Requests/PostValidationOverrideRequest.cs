using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostValidationOverrideRequest : IPostApiRequest<ValidationOverrideRequest>
    {
        public PostValidationOverrideRequest(ValidationOverrideRequest request)
        {
            Data = request;
        }

        public string PostUrl => $"validation-overrides";
        public ValidationOverrideRequest Data { get; set; }
    }

    public enum ValidationType
    {
        NotSet = 0,
        HasDaysInLearning = 1,
        IsInLearning = 2,
        HasNoDataLocks = 3,
        EmployedBeforeSchemeStarted = 4,
        EmployedAtStartOfApprenticeship = 5
    }

    public class ValidationOverrideRequest
    {
        public ValidationOverride[] ValidationOverrides { get; set; }
    }

    public class ValidationOverride
    {
        public long AccountLegalEntityId { get; set; }
        public long ULN { get; set; }
        public ValidationStep[] ValidationSteps { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }

    public class ValidationStep
    {
        public ValidationType ValidationType { get; set; }        
        public DateTime ExpiryDate { get; set; }
        public bool? Remove { get; set; }
    }
}
