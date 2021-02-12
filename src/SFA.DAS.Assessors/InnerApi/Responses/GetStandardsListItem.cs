using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardsListItem : StandardApiResponseBase
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string IFateReferenceNumber { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }
        public string Status { get; set; }
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public string EqaProviderName { get; set; }
        public string EqaProviderContactName { get; set; }
        public string EqaProviderContactEmail { get; set; }
        public string EqaProviderWebLink { get; set; }
        public string IntegratedDegree { get; set; }
        public string TrailBlazerContact { get; set; }
        public string OverviewOfRole { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string StandardPageUrl { get; set; }
    }
}