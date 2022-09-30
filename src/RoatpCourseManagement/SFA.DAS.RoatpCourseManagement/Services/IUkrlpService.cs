using SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;

namespace SFA.DAS.RoatpCourseManagement.Services
{
    public interface IUkrlpService
    {
        Task<List<ProviderAddress>> GetAddresses(UkrlpDataCommand command);
    }
}
