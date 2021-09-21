using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationStatus
{
    public class GetApplicationStatusResult
    {
        public IEnumerable<string> Sectors { get; set; }
        public IEnumerable<string> Levels { get; set; }
        public IEnumerable<string> JobRoles { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public List<ReferenceDataItem> AllSectors { get; set; }
        public List<ReferenceDataItem> AllLevels { get; set; }
        public List<ReferenceDataItem> AllJobRoles { get; set; }
        public int RemainingAmount { get; set; }
        public bool IsNamePublic { get; set; }
        public string EmployerAccountName { get; set; }
        public string Status { get; set; }
        public string JobRole { get; set; }
        public int Level { get; set; }
        public int NumberOfApprentices { get; set; }
        public int Amount { get; set; }
        public DateTime StartBy { get; set; }
    }
}