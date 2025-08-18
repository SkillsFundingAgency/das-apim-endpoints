using System;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.Application.User.Commands.UpsertUser;

public class UpsertUserCommand : IRequest
{
    public Guid Id { get; set; }
    public UserDto User { get; set; }
}