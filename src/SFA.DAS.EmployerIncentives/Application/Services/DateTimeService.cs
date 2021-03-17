using SFA.DAS.EmployerIncentives.Interfaces;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Today => DateTime.Today;
    }
}
