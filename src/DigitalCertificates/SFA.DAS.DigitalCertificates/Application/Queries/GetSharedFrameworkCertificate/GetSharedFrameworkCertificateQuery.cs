using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate
{
    public class GetSharedFrameworkCertificateQuery : IRequest<GetSharedFrameworkCertificateQueryResult>
    {
        public Guid Id { get; set; }

        public GetSharedFrameworkCertificateQuery(Guid id)
        {
            Id = id;
        }
    }
}
