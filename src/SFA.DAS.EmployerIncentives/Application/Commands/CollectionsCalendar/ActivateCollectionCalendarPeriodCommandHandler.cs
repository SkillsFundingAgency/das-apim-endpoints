using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class ActivateCollectionCalendarPeriodCommandHandler : IRequestHandler<ActivateCollectionCalendarPeriodCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;
    
        public ActivateCollectionCalendarPeriodCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(ActivateCollectionCalendarPeriodCommand command, CancellationToken cancellationToken)
        {
            var request = new ActivateCollectionCalendarPeriodRequestData
            {
                CollectionPeriodNumber = command.CollectionPeriodNumber,
                CollectionPeriodYear = command.CollectionPeriodYear,
                Active = command.Active
            };

            await _employerIncentivesService.ActivateCollectionCalendarPeriod(request);

            return Unit.Value;
        }
    }
}
