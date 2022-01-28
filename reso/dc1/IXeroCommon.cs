using Persistent.Entity.XeroEntity;
using System.Threading.Tasks;

namespace Persistent.Interface
{
    public interface IXeroCommon
    {
        bool TokenExists();
        Task DestroyXeroToken();
        Task AuthenticateXeroAccount(string code, string state);
        Task<XeroConfig> GetRequestOption(string tenantName);
    }
}
