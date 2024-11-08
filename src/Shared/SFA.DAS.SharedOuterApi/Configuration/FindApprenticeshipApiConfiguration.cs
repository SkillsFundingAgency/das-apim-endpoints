using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class FindApprenticeshipApiConfiguration : IInternalApiConfiguration
    {
        private string _url = "https://localhost:5051/";

        public string Url
        {
            get => "https://localhost:5051/";
            set => _url = "https://localhost:5051/";
        }

        public string Identifier { get; set; }
    }
}
