using MediatR;
using SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;

public class CreateMyApprenticeshipCommand : IRequest<Unit>, IRequest<string>
{
    public Guid ApprenticeId { get; set; }
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }
    public string? TrainingCode { get; set; }
    public string? StandardUId { get; set; }

    public static implicit operator CreateMyApprenticeshipCommand(GetRecentCommitmentQueryResult source) => new()
    {
        Uln = !string.IsNullOrWhiteSpace(source.Uln) ? long.Parse(source.Uln) : null,
        ApprenticeshipId = source.ApprenticeshipId,
        EmployerName = source.EmployerName,
        StartDate = source.StartDate,
        EndDate = source.EndDate,
        TrainingProviderId = source.Ukprn,
        TrainingCode = source.TrainingCode,
        StandardUId = source.StandardUId
    };

    public static implicit operator CreateMyApprenticeshipCommand(GetStagedApprenticeResponse source) => new()
    {
        Uln = source.Uln,
        ApprenticeshipId = source.ApprenticeshipId,
        EmployerName = source.EmployerName,
        StartDate = source.StartDate,
        EndDate = source.EndDate,
        TrainingProviderId = source.TrainingProviderId,
        TrainingProviderName = source.TrainingProviderName,
        TrainingCode = source.TrainingCode,
        StandardUId = source.StandardUId
    };
}
