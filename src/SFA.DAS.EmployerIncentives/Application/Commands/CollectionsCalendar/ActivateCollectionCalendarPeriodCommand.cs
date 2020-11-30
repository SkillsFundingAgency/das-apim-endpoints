using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class ActivateCollectionCalendarPeriodCommand : IRequest
    {
        public byte CollectionPeriodNumber { get; }
        public short CollectionPeriodYear { get; }

        public ActivateCollectionCalendarPeriodCommand(byte collectionPeriodNumber, short collectionPeriodYear)
        {
            CollectionPeriodNumber = collectionPeriodNumber;
            CollectionPeriodYear = collectionPeriodYear;
        }
    }
}
