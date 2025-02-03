using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
public class GetRoatpProvidersQueryResult
{
    public IEnumerable<RoatpProvider> Providers { get; set; } = Enumerable.Empty<RoatpProvider>();
}