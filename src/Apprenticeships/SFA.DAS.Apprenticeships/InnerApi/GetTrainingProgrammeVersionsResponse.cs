using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.InnerApi
{
    public class GetTrainingProgrammeVersionsResponse
    {
        public IEnumerable<TrainingProgramme> TrainingProgrammeVersions { get; set; }
    }

    public class TrainingProgramme
    {
        public string CourseCode { get; set; }
        public string Name { get; set; }
        public string StandardUId { get; set; }
        public string Version { get; set; }
        public ProgrammeType ProgrammeType { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string StandardPageUrl { get; set; }
        public List<string> Options { get; set; }
        public List<TrainingProgrammeFundingPeriod> FundingPeriods { get; set; }
        public DateTime? VersionEarliestStartDate { get; set; }
        public DateTime? VersionLatestStartDate { get; set; }
    }

    public class TrainingProgrammeFundingPeriod
    {
        public int FundingCap { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? EffectiveFrom { get; set; }
    }

    public enum ProgrammeType
    {
        Standard = 0,
        Framework = 1
    }
}
