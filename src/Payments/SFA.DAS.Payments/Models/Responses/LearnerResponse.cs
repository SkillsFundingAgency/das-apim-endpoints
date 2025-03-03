namespace SFA.DAS.Payments.Models.Responses
{
    public class LearnerResponse 
    { 
        public int Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NiNumber { get; set; }
        public Learningdelivery[] LearningDeliveries { get; set; }
    }

    public class Learningdelivery
    {
        public int AimType { get; set; }
        public DateTime LearnStartDate { get; set; }
        public DateTime LearnPlanEndDate { get; set; }
        public int FundModel { get; set; }
        public int StdCode { get; set; }
        public string DelLocPostCode { get; set; }
        public string EpaOrgID { get; set; }
        public int CompStatus { get; set; }
        public DateTime LearnActEndDate { get; set; }
        public int WithdrawReason { get; set; }
        public int Outcome { get; set; }
        public DateTime AchDate { get; set; }
        public string OutGrade { get; set; }
        public int ProgType { get; set; }
    }

}
