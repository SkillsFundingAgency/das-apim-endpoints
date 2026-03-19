using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.EmployerPR.Application.RoatpProviders.Queries.GetRoatpProviders;
public class RoatpProvider
{
    public string Name { get; set; }
    public int Ukprn { get; set; }

    public static implicit operator RoatpProvider(Provider source)
    {
        return new RoatpProvider
        {
            Name = source.Name,
            Ukprn = source.Ukprn
        };
    }
}