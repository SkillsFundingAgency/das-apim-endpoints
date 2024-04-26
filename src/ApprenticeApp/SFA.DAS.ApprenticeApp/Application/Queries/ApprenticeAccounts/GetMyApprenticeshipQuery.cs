using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts
{
    public class GetMyApprenticeshipQuery : IRequest<GetMyApprenticeshipQueryResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}