using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.OutputFile;

public class GenerateNewOutputFileApiRequest : IPostApiRequest
{
    public string PostUrl => "api/outputfile/generate";
    public object Data { get; set; }
}
