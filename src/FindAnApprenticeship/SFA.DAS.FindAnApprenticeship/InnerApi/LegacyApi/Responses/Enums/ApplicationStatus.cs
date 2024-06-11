namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums
{
    public enum ApplicationStatus
    {
        Unknown = 0,
        Saved = 5,
        Draft = 10, // 0x0000000A
        ExpiredOrWithdrawn = 15, // 0x0000000F
        Submitting = 20, // 0x00000014
        Submitted = 30, // 0x0000001E
        InProgress = 40, // 0x00000028
        Successful = 80, // 0x00000050
        Unsuccessful = 90, // 0x0000005A
        CandidateWithdrew = 100, // 0x00000064
    }
}
