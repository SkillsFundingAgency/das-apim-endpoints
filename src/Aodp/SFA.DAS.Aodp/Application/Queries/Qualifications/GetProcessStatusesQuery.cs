using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetProcessStatusesQuery : IRequest<BaseMediatrResponse<GetProcessStatusesQueryResponse>>
{
    public string QualificationReference { get; set; } = string.Empty;
}
