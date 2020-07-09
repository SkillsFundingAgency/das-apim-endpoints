using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Configuration
{
    public class CoursesApiConfiguration : IInnerApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}
