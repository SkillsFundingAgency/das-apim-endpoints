using System.Collections.Generic;

namespace SFA.DAS.Reservations.Api.Models
{
    public class GetTrainingCoursesListResponse
    {
        public IEnumerable<GetTrainingCoursesListItem> Standards { get; set; }
    }
}