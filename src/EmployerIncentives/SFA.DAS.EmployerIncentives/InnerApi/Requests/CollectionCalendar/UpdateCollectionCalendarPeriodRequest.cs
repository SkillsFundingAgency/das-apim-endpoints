using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

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
