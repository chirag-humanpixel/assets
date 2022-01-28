using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IXeroAuthentication
    {
        bool XeroTokenExists();
        Task AuthenticateXeroAccount(string code, string state);
        Task DestroyXeroToken();
    }
}
