using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

public record GetPostcodeDataQuery : IRequest<GetPostcodeDataResult>
{
    public required string Postcode { get; init; } = null!;
}