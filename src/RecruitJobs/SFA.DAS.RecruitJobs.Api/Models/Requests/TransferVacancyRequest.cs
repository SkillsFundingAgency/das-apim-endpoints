using SFA.DAS.RecruitJobs.Domain;

namespace SFA.DAS.RecruitJobs.Api.Models.Requests;

public record TransferVacancyRequest(TransferReason TransferReason);