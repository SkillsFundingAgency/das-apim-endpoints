using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetAddresses
{
    public record GetAddressesQuery : IRequest<GetAddressesQueryResult>
    {
        public required string Query { get; init; }
    }
}