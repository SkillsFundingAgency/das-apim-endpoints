using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces
{
    public interface ILiveVacancyMapper
    {
        Application.Shared.LiveVacancy Map(LiveVacancy source, GetStandardsListResponse standards);
        Application.Shared.LiveVacancy Map(GetNhsJobApiDetailResponse source, GetLocationsListResponse locations);
    }
}
