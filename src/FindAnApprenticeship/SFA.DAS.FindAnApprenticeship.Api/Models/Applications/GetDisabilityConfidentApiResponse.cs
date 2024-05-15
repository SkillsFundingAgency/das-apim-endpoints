using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class GetDisabilityConfidentApiResponse
    {
        public string EmployerName { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public bool? IsSectionCompleted { get; set; }

        public static implicit operator GetDisabilityConfidentApiResponse(GetDisabilityConfidentQueryResult source)
        {
            return new GetDisabilityConfidentApiResponse
            {
                EmployerName = source.EmployerName,
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = source.IsSectionCompleted,
            };
        }
    }
}
