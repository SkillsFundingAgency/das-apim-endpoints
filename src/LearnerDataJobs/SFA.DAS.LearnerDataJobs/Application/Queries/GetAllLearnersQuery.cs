using MediatR;
using SFA.DAS.LearnerDataJobs.Responses;

namespace SFA.DAS.LearnerDataJobs.Application.Queries;

public class GetAllLearnersQuery : IRequest<GetAllLearnersResponse>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool ExcludeApproved { get; set; }
}
