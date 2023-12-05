using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces
{
    public interface ILiveVacancyMapper
    {
        Task<Application.Shared.LiveVacancy> Map(LiveVacancy source);
    }
}
