using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar
{
    public class ActivateCollectionCalendarPeriodRequest : IPatchApiRequest<ActivateCollectionCalendarPeriodRequestData>
    {
        public ActivateCollectionCalendarPeriodRequest()
        {
        }

        public string PatchUrl => "collectionCalendar/period/active";

        public ActivateCollectionCalendarPeriodRequestData Data { get; set; }
    }
}
