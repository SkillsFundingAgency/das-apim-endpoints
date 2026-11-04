namespace SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;

public sealed record GetProviderPaymentsPriorityResponse
{
    public List<ProviderPaymentPriorityItem> ProviderPaymentPriorities { get; set; } = [];

    public sealed record ProviderPaymentPriorityItem
    {
        public string? ProviderName { get; set; }
        public long ProviderId { get; set; }
        public int PriorityOrder { get; set; }
    }
}