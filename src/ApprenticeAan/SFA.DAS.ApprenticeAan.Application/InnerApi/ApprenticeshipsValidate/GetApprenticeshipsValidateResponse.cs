using SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.ApprenticeshipsValidate;

public class GetApprenticeshipsValidateResponse
{
    public IEnumerable<GetRecentCommitmentQueryResult> Apprenticeships { get; set; } = Enumerable.Empty<GetRecentCommitmentQueryResult>();
}
