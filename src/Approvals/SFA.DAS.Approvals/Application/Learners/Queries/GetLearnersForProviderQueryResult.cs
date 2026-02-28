using System;
using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Application.Learners.Queries;

public class GetLearnersForProviderQueryResult
{
    public DateTime? LastSubmissionDate { get; set; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string EmployerName { get; set; }
    public List<LearnerSummary> Learners { get; set; }
    public int FutureMonths { get; set; }
    public List<TrainingProgramme> TrainingCourses { get; set; }
}

public class LearnerSummary
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public long Uln { get; set; }
    public string Course { get; set; }
    public DateTime StartDate { get; set; }
}