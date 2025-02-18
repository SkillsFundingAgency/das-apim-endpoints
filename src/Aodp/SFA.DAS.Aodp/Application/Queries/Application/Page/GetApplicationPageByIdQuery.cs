using MediatR;
using SFA.DAS.Aodp.Application;

public record GetApplicationPageByIdQuery(int PageOrder, Guid SectionId, Guid FormVersionId) : IRequest<BaseMediatrResponse<GetApplicationPageByIdQueryResponse>>;
