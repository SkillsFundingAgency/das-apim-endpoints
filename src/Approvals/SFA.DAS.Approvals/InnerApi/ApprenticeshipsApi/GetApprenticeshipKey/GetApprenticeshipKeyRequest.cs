using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetApprenticeshipKey
{
	public class GetApprenticeshipKeyRequest : IGetApiRequest
	{
		public readonly long ApprenticeshipId;

		public GetApprenticeshipKeyRequest(long apprenticeshipId)
		{
			ApprenticeshipId = apprenticeshipId;
		}

		public string GetUrl => $"{ApprenticeshipId}/key2";
	}
}
