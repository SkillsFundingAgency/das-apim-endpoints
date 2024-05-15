using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePreferences;
public class GetCandidatePreferencesQuery : IRequest<GetCandidatePreferencesQueryResult>
{
    public Guid CandidateId { get; set; }
}
