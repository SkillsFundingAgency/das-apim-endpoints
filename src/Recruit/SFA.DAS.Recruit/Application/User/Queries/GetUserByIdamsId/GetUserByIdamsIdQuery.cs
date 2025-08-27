using MediatR;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;

public record GetUserByIdamsIdQuery(string IdamsId) : IRequest<GetUserByIdamsIdQueryResult>;