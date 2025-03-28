﻿namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class Account
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = "";
    public string PublicHashedAccountId { get; set; } = "";
    public string DasAccountName { get; set; } = "";
    public DateTime DateRegistered { get; set; }
    public string OwnerEmail { get; set; } = "";
    public required List<ResourceViewModel> LegalEntities { get; set; } = [];
    public required List<ResourceViewModel> PayeSchemes { get; set; } = [];
    public string ApprenticeshipEmployerType { get; set; } = "";
}
