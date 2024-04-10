namespace SFA.DAS.Apprenticeships.Api.Models
{
    public class ApprovePriceChangeRequest
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. This will be set when constructed
        public string UserId { get; set; }
#pragma warning restore CS8618

        // These 2 properties are only used when a provider is approving a employer initiated price change
        public decimal? TrainingPrice { get; set; }
        public decimal? AssessmentPrice { get; set; }
    }
}
