using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;

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