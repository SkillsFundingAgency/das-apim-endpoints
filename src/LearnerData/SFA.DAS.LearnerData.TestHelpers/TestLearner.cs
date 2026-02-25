using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.TestHelpers;

#pragma warning disable CS8618 

/// <summary>
/// Contains a Learner update request along with additional fields needed to fully describe a test learner.
/// Additional fields are needed as the learner is not created inside learning inner from the update request alone.
/// </summary>
public class TestLearner
{
    /// <summary>
    /// This is the update request that would be sent by SLD 
    /// </summary>
    public UpdateLearnerRequest UpdateLearnerRequest { get; set; }

    public Guid LearningKey { get; set; }

    public string TrainingCode { get; set; }

    public int Ukprn { get; set; }

    public int FundingBandMax { get; set; }
    public string FundingLineType { get; set; }

    /// <summary>
    /// These can be left blank. If left blank the test data builder will create default instalments based on the update request.
    /// The auto generated instalments will be crudely calculated and should not be relied upon for precise values.
    /// </summary>
    public List<SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Instalment> Instalments { get; set; }

    /// <summary>
    /// These can be left blank or manually populated, there is no auto generation of additional payments at present.
    /// </summary>
    public List<SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.AdditionalPayment> AdditionalPayments { get; set; }
}

public static class TestLearnerExtensions
{
    public static void ClearProgrammes(this TestLearner testLearner)
    {
        testLearner.UpdateLearnerRequest.Delivery.OnProgramme.Clear();
    }
    public static void AddProgramme(this TestLearner testLearner,
        int ageAtStart, DateTime startDate, DateTime endDate, int? trainingPrice = null, int? endpointAssessmentPrice = null, List<CostDetails>? costs = null)
    {
        var programme = DefaultLearner.OnProgramme;

        if(trainingPrice == null && endpointAssessmentPrice == null && costs == null)
            throw new ArgumentException("Either trainingPrice and endpointAssessmentPrice, or costs must be provided");

        if (costs == null)
        {
            costs = new List<CostDetails> { new CostDetails
            {
                FromDate = startDate,
                TrainingPrice = trainingPrice,
                EpaoPrice = endpointAssessmentPrice
            } };
        }

        programme.StartDate = startDate;
        programme.ExpectedEndDate = endDate;
        programme.Costs = costs;

        testLearner.UpdateLearnerRequest.Learner.Dob = startDate.AddYears(-ageAtStart);

        testLearner.UpdateLearnerRequest.Delivery.OnProgramme.Add(programme);

    }
}

#pragma warning restore CS8618