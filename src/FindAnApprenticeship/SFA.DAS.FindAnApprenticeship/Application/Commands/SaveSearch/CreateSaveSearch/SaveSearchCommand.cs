using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch.CreateSaveSearch;

public record SaveSearchCommand(
    Guid Id,
    Guid CandidateId,
    bool DisabilityConfident,
    bool? ExcludeNational,
    int? Distance,
    string? Location,
    string SearchTerm,
    List<int>? SelectedLevelIds,
    List<int>? SelectedRouteIds,
    string UnSubscribeToken,
    List<ApprenticeshipTypes>? ApprenticeshipTypes
) : IRequest<SaveSearchCommandResult>;