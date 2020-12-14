using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class UpdateCollectionCalendarPeriodCommandHandler : IRequestHandler<UpdateCollectionCalendarPeriodCommand>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;
    
        public UpdateCollectionCalendarPeriodCommandHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(UpdateCollectionCalendarPeriodCommand command, CancellationToken cancellationToken)
        {
            var request = new UpdateCollectionCalendarPeriodRequestData
            {
                CollectionPeriodNumber = command.CollectionPeriodNumber,
                CollectionPeriodYear = command.CollectionPeriodYear,
                Active = command.Active
            };

            await _employerIncentivesService.UpdateCollectionCalendarPeriod(request);

            return Unit.Value;
        }
    }
}
