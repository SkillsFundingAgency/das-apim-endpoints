﻿using SFA.DAS.SharedOuterApi.Interfaces;

public class CreateQualificationDiscussionHistoryApiRequest : IPutApiRequest
{
    public Guid QualificationVersionId { get; set; }

    public string PutUrl => $"/api/qualifications/{QualificationVersionId}/funding-offers-history-note";
    public object Data { get; set; }
}
