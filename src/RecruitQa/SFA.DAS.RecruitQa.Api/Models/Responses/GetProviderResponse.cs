using SFA.DAS.RecruitQa.Domain.Models;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.Models.Responses;

public record GetProviderResponse
{
    public int Ukprn { get; set; }
    public string Name { get; set; }
    public string ContactUrl { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int ProviderTypeId { get; set; }
    public int StatusId { get; set; }
    public GetProviderAddress Address { get; set; }

    public static implicit operator GetProviderResponse(GetProvidersListItem source)
    {
        return new GetProviderResponse
        {
            Ukprn = source.Ukprn,
            Name = source.Name,
            Email = source.Email,
            Phone = source.Phone,
            ContactUrl = source.ContactUrl,
            ProviderTypeId = source.ProviderTypeId,
            StatusId = source.StatusId,
            Address = source.Address
        };
    }
}