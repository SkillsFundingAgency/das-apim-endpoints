using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetApprenticeshipKey
{
	public class GetLearningKeyRequest : IGetApiRequest
	{
		public readonly long ApprenticeshipId;

		public GetLearningKeyRequest(long apprenticeshipId)
		{
			ApprenticeshipId = apprenticeshipId;
		}

		public string GetUrl => $"{ApprenticeshipId}/key2";
	}
}
