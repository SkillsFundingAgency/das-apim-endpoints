using MediatR;

namespace SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
public class GetPermissionsQuery : IRequest<GetPermissionsResponse>
{
    public long? Ukprn { get; set; }
    public string? AccountLegalEntityId { get; set; }
}
