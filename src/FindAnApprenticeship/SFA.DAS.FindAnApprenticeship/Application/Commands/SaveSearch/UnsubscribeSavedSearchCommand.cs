using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public record UnsubscribeSavedSearchCommand(Guid Id) : IRequest;

//public class UnsubscribeSavedSearchCommand : IRequest
//{
//    public Guid Id { get; set; }
//}