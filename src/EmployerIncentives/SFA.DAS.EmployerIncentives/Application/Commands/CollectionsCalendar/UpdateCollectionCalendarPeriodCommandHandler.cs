using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class UpdateCollectionCalendarPeriodCommandHandler : IRequestHandler<UpdateCollectionCalendarPeriodCommand, Unit>
    {
        private readonly ICollectionCalendarService _collectionCalendarService;
    
        public UpdateCollectionCalendarPeriodCommandHandler(ICollectionCalendarService collectionCalendarService)
        {
            _collectionCalendarService = collectionCalendarService;
        }

        public async Task<Unit> Handle(UpdateCollectionCalendarPeriodCommand command, CancellationToken cancellationToken)
        {
            var request = new UpdateCollectionCalendarPeriodRequestData
            {
                PeriodNumber = command.PeriodNumber,
                AcademicYear = command.AcademicYear,
                Active = command.Active
            };

            await _collectionCalendarService.UpdateCollectionCalendarPeriod(request);

            return Unit.Value;
        }
    }
}
