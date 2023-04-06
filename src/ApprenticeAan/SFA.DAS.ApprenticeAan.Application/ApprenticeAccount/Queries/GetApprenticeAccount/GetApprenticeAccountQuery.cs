using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;

public class GetApprenticeAccountQuery : IRequest<GetApprenticeAccountQueryResult?>
{
    public Guid ApprenticeId { get; init; }
}
