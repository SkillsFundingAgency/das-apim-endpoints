using System.Net;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;

public class PostOrganisationCommand : IRequest<HttpStatusCode>
{
    public int Ukprn { get; set; }
    public required string LegalName { get; set; }
    public string? TradingName { get; set; }
    public string? CompanyNumber { get; set; }
    public string? CharityNumber { get; set; }
    public ProviderType ProviderType { get; set; }
    public int OrganisationTypeId { get; set; }
    public required string RequestingUserId { get; set; }
    public required string RequestingUserDisplayName { get; set; }
    public bool DeliversApprenticeships { get; set; }
    public bool DeliversApprenticeshipUnits { get; set; }

    public static implicit operator PostOrganisationRequest(PostOrganisationCommand source) =>
        new()
        {
            Ukprn = source.Ukprn,
            LegalName = source.LegalName,
            TradingName = source.TradingName,
            CompanyNumber = source.CompanyNumber,
            CharityNumber = source.CharityNumber,
            ProviderType = source.ProviderType,
            OrganisationTypeId = source.OrganisationTypeId,
            RequestingUserId = source.RequestingUserDisplayName
        };
}