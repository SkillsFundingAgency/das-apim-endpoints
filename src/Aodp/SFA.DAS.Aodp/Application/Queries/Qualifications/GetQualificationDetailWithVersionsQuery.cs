using MediatR;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.AODP.Application.Queries.Qualifications
{
    public class GetQualificationDetailWithVersionsQuery : IRequest<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>
    {
        public string QualificationReference { get; set; }
    }
}
