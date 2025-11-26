using System.Net;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
public record PostOrganisationCommand(int Ukprn, string LegalName, string TradingName, string CompanyNumber, string CharityNumber, ProviderType ProviderType, int OrganisationTypeId, string RequestingUserId, string RequestingUserDisplayName, bool DeliversApprenticeships, bool DeliversApprenticeshipUnits) : IRequest<HttpStatusCode>;
