using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;

public record GetRecentCommitmentQuery(string FirstName, string LastName, DateTime DateOfBirth) : IRequest<GetRecentCommitmentQueryResult?>;
