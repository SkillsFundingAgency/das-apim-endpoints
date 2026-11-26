using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharings
{
    public class GetSharingsQuery : IRequest<GetSharingsQueryResult>
    {
        public Guid UserId { get; set; }
        public Guid CertificateId { get; set; }
        public int? Limit { get; set; }
    }
}
