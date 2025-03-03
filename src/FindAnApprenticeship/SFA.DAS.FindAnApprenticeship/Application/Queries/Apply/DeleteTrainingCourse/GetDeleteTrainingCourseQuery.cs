﻿using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DeleteTrainingCourse;
public class GetDeleteTrainingCourseQuery : IRequest<GetDeleteTrainingCourseQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid TrainingCourseId { get; set; }
}
