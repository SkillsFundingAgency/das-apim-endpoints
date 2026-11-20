using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Extensions;

public static class OnProgrammeExtensions
{
    /// <summary>
    /// Returns the on programme aims for the first apprenticeship, ordered by start date.
    /// These are the aims for the same standard and agreement as the earliest starting aim, any other aims are ignored.
    /// This rule is shared by the Learning update and the FM36 block so that they stay in step.
    /// </summary>
    public static List<OnProgrammeRequestDetails> GetFirstApprenticeshipOnProgrammes(this List<OnProgrammeRequestDetails> onProgramme)
    {
        if (!onProgramme.Any())
        {
            return [];
        }

        var orderedOnProgrammes = onProgramme
            .OrderBy(x => x.StartDate)
            .ToList();

        var firstOnProgramme = orderedOnProgrammes.First();

        return orderedOnProgrammes
            .Where(x => x.StandardCode == firstOnProgramme.StandardCode &&
                        x.AgreementId == firstOnProgramme.AgreementId)
            .ToList();
    }
}
