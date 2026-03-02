using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;

public class GetCourseProvidersQuery : IRequest<GetCourseProvidersResponse>
{
    public string LarsCode { get; set; }
    public ProviderOrderBy? OrderBy { get; set; }
    public decimal? Distance { get; set; }
    public string Location { get; set; }
    public List<DeliveryMode?> DeliveryModes { get; set; }

    public List<ProviderRating?> EmployerProviderRatings { get; set; }

    public List<ProviderRating?> ApprenticeProviderRatings { get; set; }
    public List<QarRating?> Qar { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public Guid? ShortlistUserId { get; set; }
}