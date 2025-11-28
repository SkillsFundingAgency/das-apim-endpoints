using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificates
{
    public class GetCertificatesQuery : IRequest<GetCertificatesResult>
    {
        public Guid UserId { get; set; }
    }
}
