using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.EmployerPR.Application.Queries.GetRoatpProviders;
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