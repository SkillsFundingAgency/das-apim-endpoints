using MediatR;

namespace SFA.DAS.Approvals.Application.ValidateDraftLearnerDetails.Queries
{
    public class ValidateDraftLearnerDetailsQuery : IRequest<ValidateDraftLearnerDetailsQueryResult>
    {
        public string Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
