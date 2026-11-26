using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Configuration
{
    public interface IOwnerApiConfiguration : IInternalApiConfiguration { }

    public class ApprenticeCommitmentsConfiguration : IOwnerApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}