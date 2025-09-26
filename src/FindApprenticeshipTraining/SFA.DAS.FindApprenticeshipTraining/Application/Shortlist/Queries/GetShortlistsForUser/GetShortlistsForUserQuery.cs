using System;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistsForUser;

public class GetShortlistsForUserQuery : IRequest<GetShortlistsForUserResponse>
{
    public Guid UserId { get; set; }
}
