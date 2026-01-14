using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;

[ExcludeFromCodeCoverage]
public class GetRoatpProvidersQueryResult
{
    public IEnumerable<RoatpProvider> Providers { get; set; } = Enumerable.Empty<RoatpProvider>();
    public HttpStatusCode? StatusCode { get; set; }
}