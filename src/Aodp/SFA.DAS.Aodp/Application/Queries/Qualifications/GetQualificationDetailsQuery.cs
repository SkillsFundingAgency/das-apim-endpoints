using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationDetailsQuery : IRequest<BaseMediatrResponse<GetQualificationDetailsQueryResponse>>
    {
        public string QualificationReference { get; set; }
    }
}
