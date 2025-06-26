using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class CancelPendingStartDateChangeRequest : IDeleteApiRequest
{
	public CancelPendingStartDateChangeRequest(Guid apprenticeshipKey)
	{
		ApprenticeshipKey = apprenticeshipKey;
	}

	public Guid ApprenticeshipKey { get; }
	public string DeleteUrl => $"{ApprenticeshipKey}/startDateChange/pending";
}