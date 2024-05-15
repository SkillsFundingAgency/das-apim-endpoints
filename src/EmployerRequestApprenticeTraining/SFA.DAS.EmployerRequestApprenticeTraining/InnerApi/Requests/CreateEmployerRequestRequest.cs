using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    [ExcludeFromCodeCoverage]
    public class CreateEmployerRequestRequest : IPostApiRequest<CreateEmployerRequestData>
    {
        public string PostUrl => "api/employerrequest";

        public CreateEmployerRequestData Data { get; set; }

        public CreateEmployerRequestRequest(CreateEmployerRequestData data)
        {
            Data = data;
        }
    }

    [ExcludeFromCodeCoverage]
    public class CreateEmployerRequestData
    {
        public RequestType RequestType { get; set; }
    }
}
