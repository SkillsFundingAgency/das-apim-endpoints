using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData
{
    //this is the NEW version of all the requests


    public class UpdateLearningSupportApiPutRequest(Guid learningKey, UpdateLearningSupportRequest data) : IPutApiRequest<UpdateLearningSupportRequest>
    {
        public string PutUrl { get; } = $"apprenticeship/{learningKey}/learningSupport";

        public UpdateLearningSupportRequest Data { get; set; } = data;
    }

    public class UpdateLearningSupportRequest
    {
        public List<LearningSupportItem> LearningSupport { get; set; } = [];
    }

    public class LearningSupportItem
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }




    public class UpdateEnglishAndMathsApiPutRequest(Guid learningKey, UpdateEnglishAndMathsRequest data)
        : IPutApiRequest<UpdateEnglishAndMathsRequest>
    {
        public string PutUrl { get; } = $"apprenticeship/{learningKey}/english-and-maths";
        public UpdateEnglishAndMathsRequest Data { get; set; } = data;
    }

    public class UpdateEnglishAndMathsRequest
    {
        public List<EnglishAndMathsItem> EnglishAndMaths { get; set; } = [];
    }



    public class UpdateOnProgrammeApiPutRequest(Guid learningKey, UpdateOnProgrammeRequest data)
        : IPutApiRequest<UpdateOnProgrammeRequest>
    {
        public string PutUrl { get; } = $"apprenticeship/{learningKey}/on-programme";
        public UpdateOnProgrammeRequest Data { get; set; } = data;
    }

    public class UpdateOnProgrammeRequest
    {
        public Guid ApprenticeshipEpisodeKey { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public int AgeAtStartOfLearning { get; set; }
        public int? FundingBandMaximum { get; set; }
        public bool IncludesFundingBandMaximumChanges { get; set; }
        public List<PriceItem> Prices { get; set; } = [];
        public List<BreakInLearningItem> BreaksInLearning { get; set; } = [];

    }

    public class PriceItem
    {
        public Guid Key { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }


    public class BreakInLearningItem
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PriorPeriodExpectedEndDate { get; set; }
    }

    public class EnglishAndMathsItem
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Course { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public int? PriorLearningAdjustmentPercentage { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? PauseDate { get; set; }
    }
}
