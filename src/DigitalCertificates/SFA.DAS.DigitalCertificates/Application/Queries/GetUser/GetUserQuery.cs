using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserResult>
    {
        public string GovUkIdentifier { get; set; }
    }
}
