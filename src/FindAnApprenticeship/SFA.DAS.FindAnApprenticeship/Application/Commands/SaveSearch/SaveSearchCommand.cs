using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public record SaveSearchCommand(
    Guid CandidateId,
    bool DisabilityConfident,
    decimal? Distance,
    string? Location,
    string? SearchTerm,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds
)  : IRequest<SaveSearchCommandResult>;