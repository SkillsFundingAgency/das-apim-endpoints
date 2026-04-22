using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Configuration;

[ExcludeFromCodeCoverage]
public class NhsJobsConfiguration : IApiConfiguration
{
    public string Url { get; set; } = "https://www.jobs.nhs.uk/api/v1/search_xml?contractType=Apprenticeship";
}