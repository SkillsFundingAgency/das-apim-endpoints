using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetDisabilityConfidentDetailsApiResponse
    {
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public bool? IsSectionCompleted { get; set; }

        public static implicit operator GetDisabilityConfidentDetailsApiResponse(GetDisabilityConfidentDetailsQueryResult source)
        {
            return new GetDisabilityConfidentDetailsApiResponse
            {
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = source.IsSectionCompleted,
            };
        }
    }
}
