using MediatR;
using SFA.DAS.EmployerPR.InnerApi.Responses;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;

public record GetRequestQuery(Guid RequestId) : IRequest<GetRequestResponse?>;
