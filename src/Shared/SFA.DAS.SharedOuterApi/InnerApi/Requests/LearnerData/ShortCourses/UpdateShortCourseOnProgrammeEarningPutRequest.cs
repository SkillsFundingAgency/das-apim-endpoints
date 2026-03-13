using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData.ShortCourses;

public class UpdateShortCourseOnProgrammeEarningPutRequest(Guid learningKey, UpdateShortCourseOnProgrammeRequestBody data) : IPutApiRequest<UpdateShortCourseOnProgrammeRequestBody>
{
    public string PutUrl { get; } = $"/{learningKey}/shortCourses/on-programme";
    public UpdateShortCourseOnProgrammeRequestBody Data { get; set; } = data;
}

#pragma warning disable CS8618 

public class UpdateShortCourseOnProgrammeRequestBody
{
    public DateTime? WithdrawalDate { get; set; }
    public List<Milestone> Milestones { get; set; }
}

#pragma warning restore CS8618