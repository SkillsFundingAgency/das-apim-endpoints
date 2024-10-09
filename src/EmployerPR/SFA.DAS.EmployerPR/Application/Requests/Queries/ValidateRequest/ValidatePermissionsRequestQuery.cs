using MediatR;

namespace SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;

public record ValidatePermissionsRequestQuery(Guid RequestId) : IRequest<ValidatePermissionsRequestQueryResult>;
