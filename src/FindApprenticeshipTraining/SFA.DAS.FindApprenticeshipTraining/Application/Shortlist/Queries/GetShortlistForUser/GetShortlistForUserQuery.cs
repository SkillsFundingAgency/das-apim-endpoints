using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserQuery : IRequest<GetShortlistForUserResult>
    {
        public Guid ShortlistUserId { get; set; }
    }
}