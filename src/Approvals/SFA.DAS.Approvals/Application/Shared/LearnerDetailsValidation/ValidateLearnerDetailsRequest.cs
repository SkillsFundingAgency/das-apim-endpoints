using MediatR;

namespace SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation
{
    public class ValidateLearnerDetailsRequest
    {
        public string Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}