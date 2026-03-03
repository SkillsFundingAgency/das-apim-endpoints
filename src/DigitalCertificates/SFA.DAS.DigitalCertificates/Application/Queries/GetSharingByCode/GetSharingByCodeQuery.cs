using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode
{
    public class GetSharingByCodeQuery : IRequest<GetSharingByCodeQueryResult>
    {
        public Guid Code { get; set; }
    }
}
