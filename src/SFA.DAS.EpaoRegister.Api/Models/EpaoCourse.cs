﻿using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class EpaoCourse
    {
        public int Id { get; set; }
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }
        public CourseDates Periods { get; set; }

        public static explicit operator EpaoCourse(GetStandardResponse source)
        {
            return new EpaoCourse
            {
                Id = source.Id,
                Periods = (CourseDates)source.StandardDates
            };
        }
    }
}