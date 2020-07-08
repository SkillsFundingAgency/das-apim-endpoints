using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IAzureManagedIdentityApiConfiguration
    {
        string Url { get; set; }
        string Identifier { get; set; }
    }
}
