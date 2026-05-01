using MediatR;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUserByEmail;

public sealed record GetUserByEmailQuery(string Email, UserType UserType) : IRequest<GetUserByEmailQueryResult>;