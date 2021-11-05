using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string DasAccountName { get; set; }
        public int PledgeId { get; set; }
        public string Details { get; set; }
        public string StandardId { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public int Amount { get; set; }
        public bool HasTrainingProvider { get; set; }
        public IEnumerable<string> Sectors { get; set; }
        public string Postcode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessWebsite { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public DateTime CreatedOn { get; set; }
        public Standard Standard { get; set; }
        public bool IsNamePublic { get; set; }
        public string Status { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }
        //public IEnumerable<string> PledgeLocations { get; set; }
        //public IEnumerable<string> Locations { get; set; }
    }
}
