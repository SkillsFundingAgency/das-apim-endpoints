namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class EmployerIncentivesConfigurationKeys
    {
        public const string EmployerIncentivesOuterApi = "SFA.DAS.EmployerIncentives.OuterApi";
        public static string AzureActiveDirectoryApiConfiguration => $"{EmployerIncentivesOuterApi}:AzureAD";
        public static string EmployerIncentivesInnerApiConfiguration => $"{EmployerIncentivesOuterApi}:EmployerIncentivesInnerApi";
        public static string CommitmentsV2InnerApiConfiguration => $"{EmployerIncentivesOuterApi}:CommitmentsV2InnerApi";
    }
}
