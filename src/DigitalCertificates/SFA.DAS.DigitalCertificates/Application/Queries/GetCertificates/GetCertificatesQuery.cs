using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates
{
    public class GetCertificatesQuery : IRequest<GetCertificatesResult>
    {
        public Guid UserId { get; set; }
    }
}
