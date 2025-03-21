using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationVersionsForQualificationByReferenceQuery : IRequest<BaseMediatrResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>>
    {
        public string QualificationReference { get; set; }

        public GetQualificationVersionsForQualificationByReferenceQuery(string qualificationReference)
        {
            QualificationReference = qualificationReference;
        }
    }
}

