using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.Configuration
{
    public class RecruitApiConfiguration : IInternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
    }
}