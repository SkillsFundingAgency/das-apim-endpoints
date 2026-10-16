namespace SFA.DAS.LearnerData.Responses.EarningsInner;

public record BaseUpdateEarningsApiPutResponse
{
    public bool HasNewEarningsProfileVersionBeenGenerated { get; set; }
}

public record UpdateOnProgrammeEarningsApiPutResponse : BaseUpdateEarningsApiPutResponse;

public record UpdateEnglishAndMathsEarningsApiPutResponse : BaseUpdateEarningsApiPutResponse;

public record UpdateLearningSupportEarningsApiPutResponse : BaseUpdateEarningsApiPutResponse;