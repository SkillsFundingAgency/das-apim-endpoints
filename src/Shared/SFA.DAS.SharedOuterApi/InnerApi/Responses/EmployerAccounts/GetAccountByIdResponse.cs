using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

public class GetAccountByIdResponse
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; }
    public string PublicHashedAccountId { get; set; }
    public string DasAccountName { get; set; }
    public DateTime DateRegistered { get; set; }
    public string OwnerEmail { get; set; }
    public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
    public bool? NameConfirmed { get; set; }
    public bool? AddTrainingProviderAcknowledged { get; set; }
    public ResourceList LegalEntities { get; set; }
}

public class Resource
{
    public string Id { get; set; }
    public string Href { get; set; }
}

public class ResourceList : List<Resource>
{
}