using MediatR;
using SFA.DAS.ApprenticeAan.Application.Extensions;
using SFA.DAS.ApprenticeAan.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;

public class GetRecentCommitmentQueryHandler : IRequestHandler<GetRecentCommitmentQuery, GetRecentCommitmentQueryResult?>
{
    private readonly ICommitmentsV2InnerApiClient _commitmentsClient;

    public GetRecentCommitmentQueryHandler(ICommitmentsV2InnerApiClient commitmentsClient)
    {
        _commitmentsClient = commitmentsClient;
    }

    public async Task<GetRecentCommitmentQueryResult?> Handle(GetRecentCommitmentQuery request, CancellationToken cancellationToken)
    {
        var validateResponse = await _commitmentsClient.GetApprenticeshipsValidate(request.FirstName, request.LastName, request.DateOfBirth.ToApiString(), cancellationToken);

        var result = validateResponse.Apprenticeships.Count() switch
        {
            0 => null,
            1 => validateResponse.Apprenticeships.First(),
            _ => GetRecentCommitment(validateResponse.Apprenticeships)
        };

        return result;
    }

    private GetRecentCommitmentQueryResult? GetRecentCommitment(IEnumerable<GetRecentCommitmentQueryResult> commitments)
    {
        if (commitments.Select(c => c.Uln).Distinct().Count() > 1) return null;
        var orderedList = commitments.OrderByDescending(c => c.StartDate);
        var firstActive = orderedList.FirstOrDefault(c => c.PaymentStatus != 3 && c.StartDate != c.StopDate);
        return (firstActive != null) ? firstActive : orderedList.First();
    }
}
