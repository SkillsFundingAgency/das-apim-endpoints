using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetQualificationOutputFileQuery : IRequest<BaseMediatrResponse<GetQualificationOutputFileResponse>>
    {
        [AllowedCharacters(TextCharacterProfile.PersonName)]
        public string CurrentUsername { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
    }
}
