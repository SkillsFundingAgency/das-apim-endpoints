namespace SFA.DAS.EmployerPR.Application.Roatp.Queries.GetRoatpProviders;
public class GetRoatpProvidersQueryResult
{
    public IEnumerable<RoatpProvider> Providers { get; set; } = Enumerable.Empty<RoatpProvider>();
}
