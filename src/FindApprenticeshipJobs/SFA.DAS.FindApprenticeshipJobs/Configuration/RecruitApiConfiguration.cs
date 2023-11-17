using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Configuration;

[ExcludeFromCodeCoverage]
public class RecruitApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}
