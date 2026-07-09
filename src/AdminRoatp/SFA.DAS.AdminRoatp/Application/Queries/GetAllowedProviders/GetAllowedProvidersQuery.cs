using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllowedProviders;

public record GetAllowedProvidersQuery(string larsCode) : IRequest<GetAllowedProvidersResponse>;