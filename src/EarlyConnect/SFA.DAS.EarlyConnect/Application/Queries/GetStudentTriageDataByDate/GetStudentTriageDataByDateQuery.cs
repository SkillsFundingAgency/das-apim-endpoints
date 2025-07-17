using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentTriageDataByDateQuery : IRequest<List<GetStudentTriageDataByDateResult>>
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
}