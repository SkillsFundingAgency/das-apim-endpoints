﻿using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch.CreateSaveSearch;

public record SaveSearchCommandResult(Guid Id)
{
    public static SaveSearchCommandResult None => new(Guid.Empty);
    public static SaveSearchCommandResult From(PutSavedSearchApiResponse response) => new(response.Id);
}