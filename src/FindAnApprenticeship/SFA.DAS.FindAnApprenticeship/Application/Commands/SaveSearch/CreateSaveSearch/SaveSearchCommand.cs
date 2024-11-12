using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch.CreateSaveSearch;

public record SaveSearchCommand(
    Guid Id,
    Guid CandidateId,
    bool DisabilityConfident,
    decimal? Distance,
    string? Location,
    string SearchTerm,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds,
    string UnSubscribeToken
) : IRequest<SaveSearchCommandResult>;