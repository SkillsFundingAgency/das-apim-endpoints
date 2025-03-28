﻿namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public class AccountLegalEntityPermissionsModel
{
    public required long Id { get; set; }

    public required string PublicHashedId { get; set; }

    public required string Name { get; set; }

    public required long AccountId { get; set; }

    public List<ProviderPermissionsModel> Permissions { get; set; } = [];

    public List<ProviderRequestModel> Requests { get; set; } = [];

    public static implicit operator AccountLegalEntityPermissionsModel(AccountLegalEntityPermissionsResponseModel source) => new()
    {
        Id = source.Id,
        PublicHashedId = source.PublicHashedId,
        Name = source.Name,
        AccountId = source.AccountId,
        Permissions = source.Permissions.Select(a => (ProviderPermissionsModel)a).ToList(),
        Requests = source.Requests.Select(a => (ProviderRequestModel)a).ToList()
    };
}