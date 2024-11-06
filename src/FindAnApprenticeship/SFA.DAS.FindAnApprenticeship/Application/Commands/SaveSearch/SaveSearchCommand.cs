using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public record SaveSearchCommand(
    Guid CandidateId,
    bool DisabilityConfident,
    int? Distance,
    string? Location,
    string? SearchTerm,
    List<string>? SelectedLevelIds,
    List<string>? SelectedRouteIds
)  : IRequest<SaveSearchCommandResult>;