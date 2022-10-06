using CsvHelper.Configuration.Attributes;

namespace SFA.DAS.RoatpCourseManagement.Services.Models.ImportTypes
{
    public class NationalAchievementRateCsv
    {
        [Name("UKPRN")]
        public int Ukprn { get; set; }
        [Name("Age_Band")]
        public string Age { get; set; }
        [Name("Sector_Subject_Area_T2")]
        public string SectorSubjectArea { get; set; }
        [Name("Apprenticeship_Level_Grouped")]
        public string ApprenticeshipLevel { get; set; }
        [Name("Overall_Leavers")]
        public string OverallCohort { get; set; }
        [Name("Overall_Achievement_Rate")]
        public string OverallAchievementRate { get; set; }
    }
}