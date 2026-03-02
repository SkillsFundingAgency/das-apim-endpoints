using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById
{
    public class GetSharingByIdQuery : IRequest<GetSharingByIdQueryResult>
    {
        public Guid SharingId { get; set; }
        public int? Limit { get; set; }
    }
}
