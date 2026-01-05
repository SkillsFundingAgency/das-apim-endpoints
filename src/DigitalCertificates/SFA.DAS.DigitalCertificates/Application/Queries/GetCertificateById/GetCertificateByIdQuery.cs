using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetCertificateById
{
    public class GetCertificateByIdQuery : IRequest<GetCertificateByIdQueryResult>
    {
        public Guid Id { get; set; }

        public GetCertificateByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
