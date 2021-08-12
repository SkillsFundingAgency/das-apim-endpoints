using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationResult
    {
        public string Location { get; set; }
        public IEnumerable<string> Sector { get; set; }
        public string TypeOfJobRole { get; set; }
        public string Level { get; set; }
        public int NumberOfApprentices { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public DateTime StartBy { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string AboutOpportunity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
    }
}