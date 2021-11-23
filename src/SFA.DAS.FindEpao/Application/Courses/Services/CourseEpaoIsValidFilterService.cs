﻿using System;
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

            return ValidateEpaoStandardDates(courseEpao.CourseEpaoDetails.DateStandardApprovedOnRegister, courseEpao.CourseEpaoDetails.EffectiveTo, courseEpao.CourseEpaoDetails.EffectiveFrom);
        }

        public bool ValidateEpaoStandardDates(DateTime? dateStandardApprovedOnRegister, DateTime? effectiveTo, DateTime? effectiveFrom)
        {
            if (!dateStandardApprovedOnRegister.HasValue ||
                dateStandardApprovedOnRegister.Value.Date > DateTime.Today)
            {
                return false;
            }

            if (effectiveTo.HasValue &&
                effectiveTo.Value.Date < DateTime.Today)
            {
                return false;
            }

            if (!effectiveFrom.HasValue)
            {
                return false;
            }

            return true;
        }

        public bool ValidateVersionDates(DateTime? effectiveFrom, DateTime? effectiveTo)
        {

            if ((effectiveFrom.HasValue) && (effectiveTo.HasValue))
            {
                if ((effectiveFrom.Value.Date <= DateTime.Today) && (effectiveTo.Value.Date >= DateTime.Today))
                    return true;
            }

            if ((effectiveFrom.HasValue) && (!effectiveTo.HasValue))
            {
                if (effectiveFrom.Value.Date <= DateTime.Today)
                    return true;
            }

            return false;
        }
    }
}