using System;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;

namespace SFA.DAS.FindEpao.Application.Courses.Services
{
    public class CourseEpaoIsValidFilterService : ICourseEpaoIsValidFilterService
    {
        private const string LiveStatus = "Live";
        public bool IsValidCourseEpao(GetCourseEpaoListItem courseEpao)
        {
            if (!string.Equals(courseEpao.Status, LiveStatus, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (!courseEpao.CourseEpaoDetails.DateStandardApprovedOnRegister.HasValue || 
                courseEpao.CourseEpaoDetails.DateStandardApprovedOnRegister?.Date > DateTime.Today)
            {
                return false;
            }

            if (courseEpao.CourseEpaoDetails.EffectiveTo.HasValue && courseEpao.CourseEpaoDetails.EffectiveTo.Value.Date < DateTime.Today)
            {
                return false;
            }

            if (!courseEpao.CourseEpaoDetails.EffectiveFrom.HasValue)
            {
                return false;
            }

            return true;
        }
    }
}