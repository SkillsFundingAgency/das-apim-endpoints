using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar
{
    public class ActivateCollectionCalendarPeriodRequest : IPostApiRequest
    {
        public ActivateCollectionCalendarPeriodRequest()
        {
        }

        public string PostUrl => "collectionCalendar/period/activate";
        public object Data { get; set; }
    }
}
