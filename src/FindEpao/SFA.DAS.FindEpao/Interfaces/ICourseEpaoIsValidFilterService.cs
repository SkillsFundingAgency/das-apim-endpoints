using System;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Interfaces
{
    public interface ICourseEpaoIsValidFilterService
    {
        bool IsValidCourseEpao(GetCourseEpaoListItem courseEpao);
        bool ValidateEpaoStandardDates(DateTime? dateStandardApprovedOnRegister, DateTime? effectiveTo, DateTime? effectiveFrom);
        bool ValidateVersionDates(DateTime? dateVersionApproved, DateTime? effectiveFrom, DateTime? effectiveTo);
    }
}