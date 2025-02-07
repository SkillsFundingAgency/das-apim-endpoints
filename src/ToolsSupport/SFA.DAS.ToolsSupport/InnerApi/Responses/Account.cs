namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class Account
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = "";
    public string DasAccountName { get; set; } = "";
    public DateTime DateRegistered { get; set; }
    public string OwnerEmail { get; set; } = "";
    public required ResourceList LegalEntities { get; set; }
    public required ResourceList PayeSchemes { get; set; }

    //public string PublicHashedAccountId { get; set; } = "";
    //public decimal RemainingTransferAllowance { get; set; }
    //public decimal StartingTransferAllowance { get; set; }
    //public int AccountAgreementType { get; set; }
    //public string ApprenticeshipEmployerType { get; set; } = "";
    //public bool IsAllowedPaymentOnService { get; set; }
}
