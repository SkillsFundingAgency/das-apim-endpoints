using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeshipsFilterValues;

public class GetApprenticeshipsFilterValuesQuery : IRequest<GetApprenticeshipsFilterValuesQueryResult>
{
    public long? EmployerAccountId { get; set; }
    public long? ProviderId { get; set; }
}