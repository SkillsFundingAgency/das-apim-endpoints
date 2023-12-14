using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces
{
    public interface ILiveVacancyMapper
    {
        Application.Shared.LiveVacancy Map(LiveVacancy source, GetStandardsListResponse standards);
    }
}
