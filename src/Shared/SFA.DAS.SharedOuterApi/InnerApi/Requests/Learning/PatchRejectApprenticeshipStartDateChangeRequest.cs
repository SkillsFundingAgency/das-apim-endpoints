using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class PatchRejectApprenticeshipStartDateChangeRequest : IPatchApiRequest<RejectApprenticeshipStartDateChangeRequest>
{
	public PatchRejectApprenticeshipStartDateChangeRequest(
		Guid apprenticeshipKey,
		string reason)
	{
		ApprenticeshipKey = apprenticeshipKey;
		Data = new RejectApprenticeshipStartDateChangeRequest
		{
			Reason = reason
		};
	}

	public Guid ApprenticeshipKey { get; set; }
	public string PatchUrl => $"{ApprenticeshipKey}/startDateChange/reject";
	public RejectApprenticeshipStartDateChangeRequest Data { get; set; }
}

public class RejectApprenticeshipStartDateChangeRequest
{
	public string Reason { get; set; }
}