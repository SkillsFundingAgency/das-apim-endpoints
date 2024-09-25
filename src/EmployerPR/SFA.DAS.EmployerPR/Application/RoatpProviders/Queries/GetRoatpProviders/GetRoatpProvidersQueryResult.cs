namespace SFA.DAS.EmployerPR.Application.RoatpProviders.Queries.GetRoatpProviders;
public class GetRoatpProvidersQueryResult
{
    public IEnumerable<RoatpProvider> Providers { get; set; } = Enumerable.Empty<RoatpProvider>();
}
