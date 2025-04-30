using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetDiscussionHistoriesForQualificationQuery : IRequest<BaseMediatrResponse<GetDiscussionHistoriesForQualificationQueryResponse>>
{
    public string QualificationReference { get; set; } = string.Empty;
}
