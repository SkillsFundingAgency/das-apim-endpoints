using MediatR;

namespace SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

public record GetPostcodeDataQuery(string Postcode) : IRequest<GetPostcodeDataResult>;