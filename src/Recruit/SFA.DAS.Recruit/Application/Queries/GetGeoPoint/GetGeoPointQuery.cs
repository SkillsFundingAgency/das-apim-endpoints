using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetGeoPoint
{
    public record GetGeoPointQuery : IRequest<GetGeoPointQueryResult>
    {
        public required string Postcode { get; init; } = null!;
    }
}