﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }

        public class Application
        {
            public int Id { get; set; }
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
            public DateTime StartDate { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
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
            public int MaxFunding { get; set; }
            public string EmployerAccountName { get; set; }
            public bool IsLocationMatch { get; set; }
            public bool IsSectorMatch { get; set; }
            public bool IsJobRoleMatch { get; set; }
            public bool IsLevelMatch { get; set; }
            public List<GetPledgesResponse.Pledge.LocationDataItem> PledgeLocations { get; set; }
            public string SpecificLocation { get; set; }
            public string AdditionalLocations { get; set; }
            public class ApplicationLocation
            {
                public int Id { get; set; }
                public int PledgeLocationId { get; set; }
            }
        }
    }
}
