using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch
{
    public class GetCertificatesMatchQuery : IRequest<GetCertificatesMatchResult>
    {
        public Guid UserId { get; set; }
    }
}
