using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders
{
    public class GetTrainingCourseProvidersQuery : IRequest<GetProvidersListFromCourseIdResponse>
    {
        public int Id { get; set; }
        public ProviderOrderBy? OrderBy { get; set; }
        public decimal? Distance { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public List<DeliveryMode?> DeliveryModes { get; set; }

        public List<ProviderRating?> EmployerProviderRatings { get; set; }

        public List<ProviderRating?> ApprenticeProviderRatings { get; set; }
        public List<QarRating?> Qar { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid? ShortlistUserId { get; set; }
    }
}