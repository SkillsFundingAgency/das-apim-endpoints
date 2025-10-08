using System.Collections.Generic;

namespace SFA.DAS.Recruit.Domain;
public record WithdrawnVacanciesAlertModel
{
    public List<string> ClosedVacancies { get; set; } = [];
}