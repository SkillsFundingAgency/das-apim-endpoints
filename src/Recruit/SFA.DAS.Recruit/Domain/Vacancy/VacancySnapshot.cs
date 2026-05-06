namespace SFA.DAS.Recruit.Domain.Vacancy;

/*
 This exists purely for the AI code atm, the vacancy snapshot of a VacancyReview has never been deserialised in
 Recruit outer before and has a different type declaration for DisabilityConfident in Recruit/QA than it does here
 in the outer api.
 */
public class VacancySnapshot: Vacancy
{
    public new DisabilityConfident DisabilityConfident { get; set; }
}