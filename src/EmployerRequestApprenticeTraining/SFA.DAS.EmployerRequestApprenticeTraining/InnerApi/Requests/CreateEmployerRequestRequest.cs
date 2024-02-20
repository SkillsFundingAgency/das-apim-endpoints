using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.EmployerRequestApprenticeTraining.Models.Enums;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class CreateEmployerRequestRequest : IPostApiRequest<CreateEmployerRequestData>
    {
        public string PostUrl => "api/employerrequest";

        public CreateEmployerRequestData Data { get; set; }

        public CreateEmployerRequestRequest(CreateEmployerRequestData data)
        {
            Data = data;
        }
    }

    public class CreateEmployerRequestData
    {
        public RequestType RequestType { get; set; }
    }
}
