namespace SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent
{
    public class DataLockEventPeriod
    {
        public string ApprenticeshipVersion { get; set; }
        public NamedCalendarPeriod Period { get; set; }
        public bool IsPayable { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}