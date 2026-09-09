using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate
{
    public class GetSharedStandardCertificateQuery : IRequest<GetSharedStandardCertificateQueryResult>
    {
        public Guid Id { get; set; }

        public GetSharedStandardCertificateQuery(Guid id)
        {
            Id = id;
        }
    }
}
