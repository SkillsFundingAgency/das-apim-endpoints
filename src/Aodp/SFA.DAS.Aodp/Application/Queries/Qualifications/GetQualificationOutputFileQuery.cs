using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileQuery : IRequest<BaseMediatrResponse<GetQualificationOutputFileResponse>>
    {
        public string CurrentUsername { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
    }
}
