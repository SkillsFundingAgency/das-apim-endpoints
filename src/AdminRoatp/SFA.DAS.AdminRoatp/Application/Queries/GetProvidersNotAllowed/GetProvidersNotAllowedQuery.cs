using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersNotAllowed;

public record GetProvidersNotAllowedQuery(string larsCode) : IRequest<GetAllowedProvidersResponse>;
