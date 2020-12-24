using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Interfaces
{
    public interface ICourseEpaoIsValidFilterService
    {
        bool IsValidCourseEpao(GetCourseEpaoListItem courseEpao);
    }
}