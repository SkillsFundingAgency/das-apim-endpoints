namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public record PostMigrateLegacyApplicationsRequest
    {
        public required string EmailAddress { get; set; }
    }
}
