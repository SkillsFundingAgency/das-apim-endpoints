using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate
{
    public class GetFrameworkCertificateQuery : IRequest<GetFrameworkCertificateQueryResult>
    {
        public Guid Id { get; set; }

        public GetFrameworkCertificateQuery(Guid id)
        {
            Id = id;
        }
    }
}
