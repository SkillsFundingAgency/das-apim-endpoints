using System;
using System.Threading.Tasks;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Services;

public interface ITrainingProgrammeResolutionService
{
    Task<GetTrainingProgrammeResponse> GetTrainingProgrammeAsync(string courseCode, DateTime? startDate);
}
