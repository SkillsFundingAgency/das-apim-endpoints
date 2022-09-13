using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.ApprenticeshipDetails;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class GetApprenticeshipDetailsResponse
    {
        public bool HasMultipleDeliveryModelOptions { get; set; }

        public static implicit operator GetApprenticeshipDetailsResponse(GetApprenticeshipDetailsQueryResult source)
        {
            return new GetApprenticeshipDetailsResponse
            {
                HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions
            };
        }
    }
}
