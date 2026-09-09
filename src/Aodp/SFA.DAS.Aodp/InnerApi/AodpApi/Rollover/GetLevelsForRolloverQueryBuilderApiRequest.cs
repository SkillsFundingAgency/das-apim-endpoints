using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public record GetLevelsForRolloverQueryBuilderApiRequest : IGetApiRequest
{
    public string GetUrl => "api/rollover/querybuilder/levels";
}