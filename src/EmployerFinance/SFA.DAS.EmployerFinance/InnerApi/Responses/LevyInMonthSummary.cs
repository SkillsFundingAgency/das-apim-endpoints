using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses;

public record LevyInMonthSummary(DateOnly Period, decimal Amount);