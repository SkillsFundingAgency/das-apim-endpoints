﻿namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Responses;

public class GetFrameworkResponse
{
    public string? Title { get; set; }
    public int Level { get; set; }
    public string? FrameworkName { get; set; }
    public int Duration { get; set; }
}