using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Page;

public record GetApplicationPageByIdQuery(int PageOrder, Guid SectionId, Guid FormVersionId) : IRequest<BaseMediatrResponse<GetApplicationPageByIdQueryResponse>>;
