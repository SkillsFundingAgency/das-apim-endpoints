using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;

public record Standard(string LarsCode, string Title)
{
    public static implicit operator Standard(GetStandardsListItem response) => new(response.LarsCode.ToString(), response.Title);
}

