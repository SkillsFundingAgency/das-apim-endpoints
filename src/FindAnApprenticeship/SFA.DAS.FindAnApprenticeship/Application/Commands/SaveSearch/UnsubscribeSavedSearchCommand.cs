using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public class UnsubscribeSavedSearchCommand : IRequest
{
    public Guid Id { get; set; }
}