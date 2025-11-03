using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisationTypes;
public class GetOrganisationTypesQuery : IRequest<GetOrganisationTypesResponse>;
