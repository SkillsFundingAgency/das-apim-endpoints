using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate
{
    public class GetStandardCertificateQuery : IRequest<GetStandardCertificateQueryResult>
    {
        public Guid Id { get; set; }

        public GetStandardCertificateQuery(Guid id)
        {
            Id = id;
        }
    }
}
