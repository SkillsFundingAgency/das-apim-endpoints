using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetCourseProvidersModel
    {
        [FromQuery]
        public ProviderOrderBy OrderBy { get; set; }
        [FromQuery]
        public decimal? Distance { get; set; }
        [FromQuery]
        public string Location { get; set; }

        [FromQuery]
        public List<DeliveryMode?> DeliveryModes { get; set; }

        [FromQuery]
        public List<ProviderRating?> EmployerProviderRatings { get; set; }

        [FromQuery]
        public List<ProviderRating?> ApprenticeProviderRatings { get; set; }
        [FromQuery]
        public List<QarRating?> Qar { get; set; }
        [FromQuery]
        public int? Page { get; set; }
        [FromQuery]
        public int? PageSize { get; set; }

        [FromQuery]
        public Guid? ShortlistUserId { get; set; }

    }
}