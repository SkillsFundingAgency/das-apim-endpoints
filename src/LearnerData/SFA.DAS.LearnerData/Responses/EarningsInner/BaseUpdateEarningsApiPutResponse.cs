namespace SFA.DAS.LearnerData.Responses.EarningsInner;

public record BaseUpdateEarningsApiPutResponse
{
    public bool HasNewEarningsProfileVersionBeenGenerated { get; set; }
}

public record UpdateOnProgrammeEarningsApiPutResponse : BaseUpdateEarningsApiPutResponse;

public record UpdateEnglishAndMathsEarningsApiPutResponse : BaseUpdateEarningsApiPutResponse
{
    public List<Guid> CreatedCourseKeys { get; set; } = [];
    public List<Guid> ChangedCourseKeys { get; set; } = [];
    public List<Guid> RemovedCourseKeys { get; set; } = [];
    public List<Guid> ReinstatedCourseKeys { get; set; } = [];

    public List<Guid> GetUpdatedCourseKeys() =>
        CreatedCourseKeys
            .Concat(ChangedCourseKeys)
            .Concat(RemovedCourseKeys)
            .Concat(ReinstatedCourseKeys)
            .Distinct()
            .ToList();
}

public record UpdateLearningSupportEarningsApiPutResponse : BaseUpdateEarningsApiPutResponse;