using MediatR;

public record GetApplicationPageByIdQuery(int PageOrder, Guid SectionId, Guid FormVersionId) : IRequest<BaseMediatrResponse<GetApplicationPageByIdQueryResponse>>;
