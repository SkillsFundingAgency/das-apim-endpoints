using MediatR;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByDfeUserId;

public record GetUserByDfeUserIdQuery(string DfeUserId) : IRequest<GetUserByDfeUserIdQueryResult>;