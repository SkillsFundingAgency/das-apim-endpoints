using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar
{
    public class UpdateCollectionCalendarPeriodRequest
        : IPatchApiRequest<UpdateCollectionCalendarPeriodRequestData>
    {
        public UpdateCollectionCalendarPeriodRequest()
        {
        }

        public string PatchUrl => "collectionPeriods";

        public UpdateCollectionCalendarPeriodRequestData Data { get; set; }
    }
}
