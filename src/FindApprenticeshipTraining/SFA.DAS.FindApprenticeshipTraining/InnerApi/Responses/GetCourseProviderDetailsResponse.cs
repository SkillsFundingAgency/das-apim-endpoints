using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class GetCourseProviderDetailsResponse
{
    public int Ukprn { get; set; }
    public string ProviderName { get; set; }
    public ShortProviderAddressModel Address { get; set; }
    public ContactModel Contact { get; set; }
    public string CourseName { get; set; }
    public CourseType CourseType { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public int Level { get; set; }
    public string LarsCode { get; set; }
    public string IFateReferenceNumber { get; set; }
    public QarModel QAR { get; set; }
    public ReviewModel Reviews { get; set; }
    public IEnumerable<LocationModel> Locations { get; set; } = [];
    public Guid? ShortlistId { get; set; }
}
