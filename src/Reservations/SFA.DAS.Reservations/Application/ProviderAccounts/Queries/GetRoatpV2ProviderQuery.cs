using MediatR;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public class GetRoatpV2ProviderQuery : IRequest<bool>
{
    public int Ukprn { get; set; }
}