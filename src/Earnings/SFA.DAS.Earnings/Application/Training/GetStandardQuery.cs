using MediatR;
using SFA.DAS.Earnings.InnerApi;

namespace SFA.DAS.Earnings.Application.Training;

public class GetStandardQuery(string courseCode) : IRequest<GetStandardsListItem>
{
    public string CourseCode { get; set; } = courseCode;
}