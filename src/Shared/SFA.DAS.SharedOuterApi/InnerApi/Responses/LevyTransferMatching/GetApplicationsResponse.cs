using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public int TotalApplications { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }

        public class Application
        {
            public int Id { get; set; }
            public long EmployerAccountId { get; set; }
            public string DasAccountName { get; set; }
            public int PledgeId { get; set; }
            public string Details { get; set; }
            public string StandardId { get; set; }
            public string StandardTitle { get; set; }
            public int StandardLevel { get; set; }
            public int StandardDuration { get; set; }
            public int StandardMaxFunding { get; set; }
            public string StandardRoute { get; set; }

            public int NumberOfApprentices { get; set; }
            public int NumberOfApprenticesUsed { get; set; }
            public DateTime StartDate { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
            public int CurrentFinancialYearAmount { get; set; }
            public bool HasTrainingProvider { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string BusinessWebsite { get; set; }
            public IEnumerable<string> EmailAddresses { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsNamePublic { get; set; }
            public string Status { get; set; }
            public IEnumerable<ApplicationLocation> Locations { get; set; }
            public string JobRole { get; set; }
            public int PledgeRemainingAmount { get; set; }
            public string EmployerAccountName { get; set; }
            public bool MatchLocation { get; set; }
            public bool MatchSector { get; set; }
            public bool MatchJobRole { get; set; }
            public bool MatchLevel { get; set; }
            public int MatchPercentage { get; set; }
            public List<GetPledgesResponse.Pledge.LocationDataItem> PledgeLocations { get; set; }
            public string SpecificLocation { get; set; }
            public string AdditionalLocations { get; set; }
            public long SenderEmployerAccountId { get; set; }
            public string SenderEmployerAccountName { get; set; }
            public IEnumerable<CostProjection> CostProjections { get; set; }
            public AutomaticApprovalOption PledgeAutomaticApprovalOption { get; set; }

            public class ApplicationLocation
            {
                public int Id { get; set; }
                public int PledgeLocationId { get; set; }
            }

            public class CostProjection
            {
                public string FinancialYear { get; set; }
                public int Amount { get; set; }
            }
        }
    }
}
