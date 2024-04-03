using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
public class GetCandidateNameQuery : IRequest<GetCandidateNameQueryResult>
{
    public string GovUkIdentifier { get; set; }
}
