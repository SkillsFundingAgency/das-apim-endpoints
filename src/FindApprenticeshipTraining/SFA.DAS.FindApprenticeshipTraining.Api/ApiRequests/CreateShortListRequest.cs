using System;

namespace SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;

public class CreateShortListRequest
{
    public Guid ShortlistUserId { get; set; }
    public int LarsCode { get; set; }
    public string LocationName { get; set; }
    public int Ukprn { get; set; }
}