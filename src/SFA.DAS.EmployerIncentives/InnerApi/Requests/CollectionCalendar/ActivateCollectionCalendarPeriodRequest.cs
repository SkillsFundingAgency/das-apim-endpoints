using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar
{
    public class ActivateCollectionCalendarPeriodRequest : IPatchApiRequest<ActivateCollectionCalendarPeriodRequestData>
    {
        public ActivateCollectionCalendarPeriodRequest()
        {
        }

        public string PatchUrl => "collectionCalendar/period/activate";

        public ActivateCollectionCalendarPeriodRequestData Data { get; set; }
    }
}
