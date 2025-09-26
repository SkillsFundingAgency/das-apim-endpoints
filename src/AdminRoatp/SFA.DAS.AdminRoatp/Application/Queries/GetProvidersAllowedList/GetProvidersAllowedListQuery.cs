using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedList;
public record GetProvidersAllowedListQuery(string? sortColumn = null, string? sortOrder = null) : IRequest<GetProvidersAllowedListQueryResponse>;