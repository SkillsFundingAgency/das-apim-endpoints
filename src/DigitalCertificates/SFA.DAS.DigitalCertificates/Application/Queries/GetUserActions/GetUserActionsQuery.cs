using MediatR;
using System;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQuery : IRequest<GetUserActionsQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
