using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.InnerApi;
public class LearnerDataInnerApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; } = "";
    public string Identifier { get; set; } = "";
}