using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;

public class CancelPendingStartDateChangeRequest : IDeleteApiRequest
{
	public CancelPendingStartDateChangeRequest(Guid apprenticeshipKey)
	{
		ApprenticeshipKey = apprenticeshipKey;
	}

	public Guid ApprenticeshipKey { get; }
	public string DeleteUrl => $"{ApprenticeshipKey}/startDateChange/pending";
}