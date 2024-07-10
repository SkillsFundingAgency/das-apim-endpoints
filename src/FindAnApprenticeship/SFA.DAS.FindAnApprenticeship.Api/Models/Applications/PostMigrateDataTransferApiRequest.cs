namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public record PostMigrateDataTransferApiRequest
    {
        public required string EmailAddress { get; set; }
    }
}
