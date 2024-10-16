using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;

public record GetRequestQuery(Guid RequestId) : IRequest<GetRequestQueryResult?>;
