using System;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetCourseListItem
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
        public int MaxFunding { get; set; }
        public int TypicalDuration { get; set; }
        public bool IsActive { get; set; }
        public StandardDate StandardDates { get; set; }
        public string EqaProviderName { get; set; }
        public string EqaProviderContactName { get; set; }
        public string EqaProviderContactEmail { get; set; }
        public string EqaProviderWebLink { get; set; }
        public string IntegratedDegree { get; set; }
        public string TrailBlazerContact { get; set; }
        public string OverviewOfRole { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string StandardPageUrl { get; set; }


        public static implicit operator GetCourseListItem(GetStandardsListItem source)
        {
            return new GetCourseListItem
            {
                StandardUId = source.StandardUId,
                LarsCode = source.LarsCode,
                IFateReferenceNumber = source.IFateReferenceNumber,
                Title = source.Title,
                Level = source.Level,
                Route = source.Route,
                Status = source.Status,
                EarliestStartDate = source.EarliestStartDate,
                LatestStartDate = source.LatestStartDate,
                LatestEndDate = source.LatestEndDate,
                ApprovedForDelivery = source.ApprovedForDelivery,
                MaxFunding = source.MaxFunding,
                TypicalDuration = source.TypicalDuration,
                IsActive = source.IsActive,
                StandardDates = source.StandardDates,
                EqaProviderContactEmail = source.EqaProviderContactEmail,
                EqaProviderContactName = source.EqaProviderContactName,
                EqaProviderName = source.EqaProviderName,
                EqaProviderWebLink = source.EqaProviderWebLink,
                IntegratedDegree = source.IntegratedDegree,
                TrailBlazerContact = source.TrailBlazerContact,
                OverviewOfRole = source.OverviewOfRole,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                StandardPageUrl = source.StandardPageUrl
            };
        }

        public class StandardDate
        {
            public DateTime? LastDateStarts { get; set; }

            public DateTime? EffectiveTo { get; set; }

            public DateTime EffectiveFrom { get; set; }

            public static implicit operator StandardDate(SharedOuterApi.InnerApi.Responses.StandardDate source)
            {
                return source == null ? null :
                    new StandardDate
                    {
                        EffectiveFrom = source.EffectiveFrom,
                        EffectiveTo = source.EffectiveTo,
                        LastDateStarts = source.LastDateStarts
                    };
            }
        }
    }
}