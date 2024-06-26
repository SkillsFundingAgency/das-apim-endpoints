﻿using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class PostCandidateApiResponse
{
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
}