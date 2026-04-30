using System.Collections.Generic;

namespace SFA.DAS.AparRegister.Api.Models;

/// <summary>
/// ProviderList
/// </summary>
public class ProvidersApiResponse
{
    /// <summary>
    /// Providers
    /// </summary>
    public IEnumerable<ProviderModel> Providers { get; set; }
}
