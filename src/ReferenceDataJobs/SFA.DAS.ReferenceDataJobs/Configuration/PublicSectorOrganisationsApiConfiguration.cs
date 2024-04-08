using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ReferenceDataJobs.Configuration;

[ExcludeFromCodeCoverage]
public class PublicSectorOrganisationsApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}
