using System;
using MediatR;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public record GetApprenticeshipUnitVersionQuery(string CourseCode, DateTime StartDate)
    : IRequest<GetApprenticeshipUnitVersionResult>;
