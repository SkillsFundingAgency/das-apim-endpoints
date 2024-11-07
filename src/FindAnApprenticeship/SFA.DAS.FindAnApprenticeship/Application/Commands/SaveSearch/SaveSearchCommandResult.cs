using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.SaveSearch;

public record SaveSearchCommandResult(Guid Id)
{
    public static SaveSearchCommandResult None => new(Guid.Empty);
    public static SaveSearchCommandResult From(PostSavedSearchApiResponse response) => new (response.Id);
}