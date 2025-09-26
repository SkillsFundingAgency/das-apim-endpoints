using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetRplRequirements
{
    public class GetRplRequirementsQuery : IRequest<GetRplRequirementsResult>
    {
        public string CourseId { get; set; }
    }
} 