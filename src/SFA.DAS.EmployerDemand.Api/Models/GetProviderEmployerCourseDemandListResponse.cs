using System;
using System.Collections.Generic;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetProviderEmployerCourseDemandListResponse
    {
        public IEnumerable<GetProviderEmployerDemandDetailsListItem> ProviderEmployerDemandDetailsList { get; set; }
        public GetCourseListItem TrainingCourse { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
        public GetLocationSearchResponseItem Location { get; set; }
        public GetProviderContactDetails ProviderContactDetails { get; set; }
    }

    public class GetProviderEmployerDemandDetailsListItem
    {
        public Guid Id { get ; set ; }
        public int ApprenticesCount { get ; set ; }
        public GetLocationSearchResponseItem Location { get ; set ; }


        public static implicit operator GetProviderEmployerDemandDetailsListItem(GetEmployerCourseProviderDemandResponse source)
        {
            return new GetProviderEmployerDemandDetailsListItem
            {
                Id = source.Id,
                Location = source.Location,
                ApprenticesCount = source.ApprenticesCount
            };
        }
    }
}