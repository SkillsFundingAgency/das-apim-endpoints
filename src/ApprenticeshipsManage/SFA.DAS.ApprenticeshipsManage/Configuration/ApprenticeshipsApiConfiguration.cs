using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeshipsManage.Configuration;
public class ApprenticeshipsApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; } = "";
    public string Identifier { get; set; } = "";
}