using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetDateOfBirth;
public class GetDateOfBirthQuery : IRequest<GetDateOfBirthQueryResult>
{
    public Guid CandidateId { get; set; }
}
