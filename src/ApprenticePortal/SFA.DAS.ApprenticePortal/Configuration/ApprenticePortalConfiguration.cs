using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.Configuration
{
    public interface IOwnerApiConfiguration : IInternalApiConfiguration { }

    public class ApprenticePortalConfiguration : IOwnerApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}