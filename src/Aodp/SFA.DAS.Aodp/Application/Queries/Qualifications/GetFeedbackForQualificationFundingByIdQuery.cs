using MediatR;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.Aodp.Application.Queries.Application.Review
{
    public class GetFeedbackForQualificationFundingByIdQuery : IRequest<BaseMediatrResponse<GetFeedbackForQualificationFundingByIdQueryResponse>>
    {
        public Guid QualificationVersionId { get; set; }

        public GetFeedbackForQualificationFundingByIdQuery(Guid qualificationVersionId)
        {
            QualificationVersionId = qualificationVersionId;
        }
    }
}

