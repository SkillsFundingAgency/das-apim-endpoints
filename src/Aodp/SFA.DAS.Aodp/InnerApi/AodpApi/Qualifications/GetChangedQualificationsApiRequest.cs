using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;

public class GetChangedQualificationsApiRequest : IGetApiRequest
{
    public string GetUrl => $"api/qualifications/changed";
}
