using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public class GetApprenticeQuery : IRequest<GetApprenticeQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}