using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetCohortSupportApprenticeshipsQuery : IRequest<GetCohortSupportApprenticeshipsQueryResult?>
{
    public long CohortId { get; set; }
}