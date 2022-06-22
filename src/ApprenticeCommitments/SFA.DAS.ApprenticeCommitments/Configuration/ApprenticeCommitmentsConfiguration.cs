using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Configuration
{
    public interface IOwnerApiConfiguration : IInternalApiConfiguration { }

    public class ApprenticeCommitmentsConfiguration : IOwnerApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}