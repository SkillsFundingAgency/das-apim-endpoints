using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.OutputFile;

public class GetPreviousOutputFilesApiRequest : IGetApiRequest
{
    public string GetUrl => "api/outputfiles";
}
